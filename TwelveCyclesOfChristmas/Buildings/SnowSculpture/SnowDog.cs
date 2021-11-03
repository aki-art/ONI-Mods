using KSerialization;
using System;
using UnityEngine;

namespace TwelveCyclesOfChristmas.Buildings.SnowSculpture
{
    public class SnowDog : KMonoBehaviour
    {
        [Serialize]
        public int petCapacity;

        [MyCmpReq]
        private KBatchedAnimController body;

        private KBatchedAnimController head;
        private Mesh neck;
        GameObject neckGo;

        KAnimLink link;

        const float NECK_SCALE_Y = 0.23f;
        const float NECK_SCALE_X = 0.64f;

        static Vector3 neckOffset = new Vector3(-0.46f, 1.2f, -0.5f);

        protected override void OnSpawn()
        {
            base.OnSpawn();
            neck = CreateNeckMesh();
            ExtrudeDog();
            Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenu);
        }

        private void OnRefreshUserMenu(object obj)
        {
            KIconButtonMenu.ButtonInfo button;
            button = new KIconButtonMenu.ButtonInfo("action_switch_toggle", "Pet", OnPet);
            Game.Instance.userMenu.AddButton(gameObject, button);
        }

        private void OnPet()
        {
            petCapacity++;
            ExtrudeDog();
            ScaleNeckMesh(petCapacity, petCapacity);
            DrawNeck();
        }

        private void ExtrudeDog()
        {
            if (body is null) return;

            if (head is null)
            {
                head = FXHelpers.CreateEffect("snowsculpture_kanim", transform.position, transform, false, Grid.SceneLayer.BuildingFront);
                head.Play("dog_head");
                head.gameObject.SetActive(true);
                link = new KAnimLink(body, head);
                head.OverlayColour = body.OverlayColour;
                head.TintColour = body.TintColour;
                
            }

            head.transform.position = transform.position + new Vector3(0, NECK_SCALE_Y) * petCapacity;
            head.SetDirty();

            ScaleNeckMesh(petCapacity, petCapacity);
            DrawNeck();
        }

        // you will never guess: this method... draws the neck
        private void DrawNeck()
        {
            Graphics.DrawMesh(neck, transform.position + neckOffset, Quaternion.identity, ModAssets.dogMaterial, (int)Grid.SceneLayer.BuildingBack);
        }

        // vertically scales the neck
        private void ScaleNeckMesh(float yScale, int textureRepeatY)
        {
            if (neck is null) return;

            /*
             * this would work with tiling shaders, kbtached is not one.
            float height = NECK_SCALE_Y * yScale;

            neck.vertices = new Vector3[4]
            {
                new Vector3(0, 0, 0),
                new Vector3(NECK_SCALE_X, 0, 0),
                new Vector3(0, height, 0),
                new Vector3(NECK_SCALE_X, height, 0)
            };

            gameObject.AddOrGet<MeshRenderer>().material.SetTextureScale("_MainTex", new Vector2(1f, textureRepeatY));
            neck.RecalculateBounds();
            */
        }

        // Create simple quad
        private Mesh CreateNeckMesh()
        {
            float width = 0.64f;
            float height = 0.23f;

            neckGo = new GameObject();
            neckGo.transform.SetParent(transform);
            neckGo.SetActive(true);
            neckGo.transform.localPosition = neckOffset;

            MeshRenderer meshRenderer = neckGo.AddOrGet<MeshRenderer>();
            meshRenderer.sharedMaterial = new Material(Shader.Find("Klei/Unlit Transparent"));
            meshRenderer.sharedMaterial.SetTexture("_MainTex", ModAssets.dog);
            meshRenderer.sharedMaterial.SetColor("_Color", body.OverlayColour); // set this one manually

            body.OnHighlightChanged += color => meshRenderer.sharedMaterial.SetColor("_Color", color + body.TintColour);
            body.OnOverlayColourChanged += color => meshRenderer.sharedMaterial.SetColor("_Color", body.TintColour * (Color)color);
            body.OnTintChanged += color => meshRenderer.sharedMaterial.SetColor("_Color", color);

            MeshFilter meshFilter = neckGo.AddComponent<MeshFilter>();

            Mesh mesh = new Mesh
            {
                vertices = new Vector3[4]
                {
                    new Vector3(0, 0, 0),
                    new Vector3(width, 0, 0),
                    //new Vector3(0, height / 2f, 0),
                    //new Vector3(width, height / 2f, 0),
                    new Vector3(0, height, 0),
                    new Vector3(width, height, 0)
                },

                triangles = new int[6]
                {
                    0, 2, 1,
                    2, 3, 1,
                    //2, 4, 3,
                   // 4, 5, 3
                },

                normals = new Vector3[4]
                {
                    -Vector3.forward,
                    -Vector3.forward,
                    -Vector3.forward,
                    -Vector3.forward,
                    //-Vector3.forward,
                    //-Vector3.forward
                },

                uv = new Vector2[4]
                {
                    new Vector2(0f, 0f),
                    new Vector2(1f, 0f),
                    //new Vector2(0f, .5f),
                    //new Vector2(1f, .5f),
                    new Vector2(0f, 1f),
                    new Vector2(1f, 1f)
                }
            };

            meshFilter.mesh = mesh;

            return mesh;
        }
    }
}
