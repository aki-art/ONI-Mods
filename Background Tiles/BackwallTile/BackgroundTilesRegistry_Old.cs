
/*namespace BackgroundTiles.Buildings
{
    class BackgroundTilesRegistry_Old
    {
		static GameObject baseTemplate;
		public static Dictionary<BuildingDef, BuildingDef> tiles = new Dictionary<BuildingDef, BuildingDef>();

		public static void SetBaseTemplate()
		{
			baseTemplate = Traverse.Create(BuildingConfigManager.Instance).Field<GameObject>("baseTemplate").Value;
		}

        public static void RegisterTile(IBuildingConfig original, BuildingDef originalDef)
        {
			string ID = Mod.ID + "_" + originalDef.Tag + "Wall";
			RegisterBuilding(original, originalDef, ID);

            //ModUtil.AddBuildingToPlanScreen(Mod.BackwallCategory, ID);
        }

		static void RegisterStrings(string original, string newTag)
		{
			string key = $"STRINGS.BUILDINGS.PREFABS.{newTag.ToUpperInvariant()}";
			string originalKey = $"STRINGS.BUILDINGS.PREFABS.{original.ToUpperInvariant()}";

			Log.Debuglog(key, originalKey);

			Strings.Add(key + ".NAME", $"Backwall ({Strings.Get(originalKey + ".NAME")})"); // todo: also translatable
			Strings.Add(key + ".DESC", Strings.Get(originalKey + ".DESC"));
			Strings.Add(key + ".EFFECT", Strings.Get(originalKey + ".EFFECT"));
		}

		private static void RegisterTech(string originalID, string ID)
		{
			Log.Debuglog("RegisterTech");
			if (Db.Get().Techs is null) return;
			Log.Debuglog("techs exists");

			Tech tech = Db.Get().Techs.TryGetTechForTechItem(originalID);

			if (tech is null) return;
			Log.Debuglog("tech found", tech.Id);

			tech.AddUnlockedItemIDs(ID);
			Log.Debuglog("tech added");
		}

		// manually registering
		public static void RegisterBuilding(IBuildingConfig original, BuildingDef originalDef, string ID)
		{
			if (!DlcManager.IsDlcListValidForCurrentContent(original.GetDlcIds()))
			{
				return;
			}

			BuildingDef def = CreateBuildingDef(originalDef, ID);
			RegisterStrings(originalDef.PrefabID, def.PrefabID); // Adding strings early, some things below will already try to access them

			def.RequiredDlcIds = original.GetDlcIds();
			GameObject gameObject = Object.Instantiate(baseTemplate);
            Object.DontDestroyOnLoad(gameObject);

			gameObject.GetComponent<KPrefabID>().PrefabTag = def.Tag;

			gameObject.name = def.PrefabID + "Template";
			gameObject.GetComponent<Building>().Def = def;
			gameObject.GetComponent<OccupyArea>().OccupiedCellsOffsets = def.PlacementOffsets;

			ConfigureBuildingTemplate(gameObject, def.Tag);

			def.BuildingComplete = BuildingLoader.Instance.CreateBuildingComplete(gameObject, def);

			def.BuildingUnderConstruction = BuildingLoader.Instance.CreateBuildingUnderConstruction(def);
			def.BuildingUnderConstruction.name = BuildingConfigManager.GetUnderConstructionName(def.BuildingUnderConstruction.name);

			def.BuildingPreview = BuildingLoader.Instance.CreateBuildingPreview(def);
			def.BuildingPreview.name += "Preview";

			RegisterTech(originalDef.PrefabID, def.PrefabID); // important to do before def.PostProcess()
			def.PostProcess();

			DoPostConfigureComplete(def.BuildingComplete);
			DoPostConfigureUnderConstruction(def.BuildingUnderConstruction);

			Assets.AddBuildingDef(def);
			tiles.Add(def, originalDef);


			KAnimFile kanim = def.AnimFiles[0];

			foreach (Texture2D tex in kanim.textureList)
			{
				Log.Debuglog(tex.name);
			}

			Sprite sprite = Def.GetUISpriteFromMultiObjectAnim(kanim);

			int width = 128 + 40;
			int height = 128 + 40;
			int xOffset = 20;
			int yOffset = 20;

			//Texture2D newTex = new Texture2D(width, height, def.BlockTileAtlas.texture.format);
			Texture2D spriteReadable = new Texture2D(sprite.texture.width, sprite.texture.height, TextureFormat.RGBA32, sprite.texture.mipmapCount, false);
			Graphics.ConvertTexture(sprite.texture, spriteReadable);

			Texture2D tex1 = new Texture2D(spriteReadable.width, spriteReadable.height);

			/*
			Color mod = new Color(1f, 0, 0);

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					tex1.SetPixel(x, y, spriteReadable.GetPixel(x, y) + mod);
				}
			}

			tex1.Apply();
			*/


//Graphics.CopyTexture(def.BlockTileAtlas.texture, tileSetEditable);
//int mm = tileSetEditable.loadedMipmapLevel;
//Graphics.CopyTexture(def.BlockTileAtlas.texture, 0, def.BlockTileAtlas.texture.loadedMipmapLevel, 20, 20, 168, 168, tileSetEditable, 0, mm, 0, 0);

//tileSetEditable.SetPixels(def.BlockTileAtlas.texture.GetPixels());
//Texture2D tileSetRGBA32 = new Texture2D(tileSetEditable.width, tileSetEditable.height, TextureFormat.RGBA32, false);
//Texture2D test = ConvertFormat(tileSetRGBA32, tileSetEditable.format);

//Graphics.ConvertTexture(tileSetEditable, tileSetRGBA32);

//Texture2D cropped = new Texture2D(width, height, TextureFormat.RGBA32, false);
//Graphics.CopyTexture(def.BlockTileAtlas.texture, 0, 0, 20, 20, 168, 168, cropped, 0, 0, 0, 0);


//cropped.SetPixels(tileSetRGBA32.GetPixels()); //xOffset, yOffset, width, height));
//cropped.Apply();

//Log.Debuglog(tileSetRGBA32.width, tileSetRGBA32.height);
//tileSetRGBA32.Resize(width, height);
//tileSetRGBA32.Apply();
//Log.Debuglog(tileSetRGBA32.width, tileSetRGBA32.height);

//Texture2D test = new Texture2D(sprite.texture.width,  sprite.texture.height, sprite.texture.format, 1, true);


//Graphics.CopyTexture(test, sprite.texture);
//
//tileSet.Apply();

/*
Graphics.CopyTexture(
    def.BlockTileAtlas.texture, 
    0,
    0,
    20,
    20,
    168,
    168,
    newTex,
    0,
    0,
    0,
    0);

*/

//public static void CopyTexture(Texture src, int srcElement, int srcMip, int srcX, int srcY, int srcWidth, int srcHeight, Texture dst, int dstElement, int dstMip, int dstX, int dstY);


//Graphics.ConvertTexture(sprite.texture, newTex);

//Color[] c = tileSet.GetPixels(xOffset, yOffset, width, height);
//newTex.SetPixels(c);


/*
for (int x = 0; x < width; x++)
{
    for (int y = 0; y < height; y++)
    {
        newTex.SetPixel(x, y, tileSet.GetPixel(xOffset + x, yOffset + y));
    }
}



//Sprite newSprite = Sprite.Create(newTex, new Rect(0, 0, 168, 168), sprite.pivot, 128);
//Sprite newSprite = Sprite.Create(newTex, new Rect(0, 0, width, height), sprite.pivot, sprite.pixelsPerUnit);
//Sprite newSprite = Sprite.Create(spriteReadable, new Rect(0, 0, spriteReadable.width, spriteReadable.height), sprite.pivot, sprite.pixelsPerUnit);
//def.BlockTileAtlas.items[0].

int num3 = def.BlockTileAtlas.items[0].name.Length - 4 - 8;
int startIndex = num3 - 1 - 8;

Vector4 uvBox = Vector4.zero;

for (int k = 0; k < def.BlockTileAtlas.items.Length; k++)
{
    TextureAtlas.Item item = def.BlockTileAtlas.items[k];

    string value = item.name.Substring(startIndex, 8);
    int requiredConnections = Convert.ToInt32(value, 2);

    if((Bits)requiredConnections == (Bits.Up | Bits.Down | Bits.Left | Bits.Right))
    {
        Log.Debuglog("ITEM ID", k);
        uvBox = item.uvBox;
        Log.Debuglog(item.vertices);
    }
}

Log.Debuglog(uvBox);
Log.Debuglog(uvBox.x, uvBox.y, uvBox.z, uvBox.w);

//Sprite newSprite = Sprite.Create(def.BlockTileAtlas.texture, new Rect(20, 20, width, height), sprite.pivot, sprite.pixelsPerUnit);
int tw = def.BlockTileAtlas.texture.width;
int th = def.BlockTileAtlas.texture.height;

// inverted coordinate system
float x = uvBox.x * tw;
float y = uvBox.w * th;
float w = uvBox.z * tw - x;
float h = uvBox.y * th - y;

Sprite newSprite = Sprite.Create(def.BlockTileAtlas.texture, new Rect(x, y, w, h), sprite.pivot, sprite.pixelsPerUnit * 1.5f, 0, SpriteMeshType.FullRect, new Vector4(0, 0, 100, 100));


Tuple<KAnimFile, string, bool> key = new Tuple<KAnimFile, string, bool>(kanim, "ui", false);
var sprites = Traverse.Create(typeof(Def)).Field<Dictionary<Tuple<KAnimFile, string, bool>, Sprite>>("knownUISprites");
sprites.Value[key] = newSprite;
}

private static Texture2D ConvertFormat(Texture2D src, TextureFormat format)
{
Texture2D result = new Texture2D(src.width, src.height, format, false);
result.SetPixels(src.GetPixels());
result.Apply();

return result;
}

private static EffectorValues GetDecor(float originalDecor, int range)
{
int decor = Mathf.FloorToInt(originalDecor * Mod.Settings.DecorModifier);

if(Mod.Settings.CapDecorAt0) { 
    decor = Mathf.Max(decor, 0);
}

return new EffectorValues(decor, range);
}

private static float[] GetMass(float[] originalMass)
{
float[] result = new float[originalMass.Length];
for (int i = 0; i < originalMass.Length; i++)
{
    result[i] = originalMass[i] * Mod.Settings.MassModifier;
    result[i] = Math.Max(result[i], 1); // need at least 1
}

return result;
}

public static BuildingDef CreateBuildingDef(BuildingDef original, string ID)
{

BuildingDef def = BuildingTemplates.CreateBuildingDef(
    ID,
    1,
    1,
    original.AnimFiles[0].name,
    Mathf.FloorToInt(original.HitPoints * Mod.Settings.HitPointModifier),
    original.ConstructionTime * Mod.Settings.MassModifier,
    GetMass(original.Mass),
    original.MaterialCategory,
    original.BaseMeltingPoint,
    BuildLocationRule.NotInTiles,
    GetDecor(original.BaseDecor, 2),
    NOISE_POLLUTION.NONE
);

def.IsFoundation = false;
def.TileLayer = ObjectLayer.Backwall;
def.ReplacementLayer = ObjectLayer.Backwall;

def.ReplacementTags = new List<Tag>
{
    //GameTags.
};

def.Floodable = false;
def.Overheatable = false;
def.Entombable = false;

def.UseStructureTemperature = false;
def.BaseTimeUntilRepair = -1f;

def.AudioCategory = original.AudioCategory;
def.AudioSize = "small";

def.ObjectLayer = ObjectLayer.Backwall;
def.SceneLayer = Grid.SceneLayer.Backwall;
def.isKAnimTile = true;

def.BlockTileIsTransparent = true; // otherwise it does not want to render solid tiles
def.BlockTileMaterial = original.BlockTileMaterial;
def.BlockTileAtlas = original.BlockTileAtlas;
def.BlockTilePlaceAtlas = original.BlockTilePlaceAtlas; // todo: replace with custom
def.BlockTileShineAtlas = original.BlockTileShineAtlas;

// leaving these null so they are not rendered
// def.DecorBlockTileInfo = null;
// def.DecorPlaceBlockTileInfo = null;

return def;
}

static void ConfigureBuildingTemplate(GameObject go, Tag tag)
{
GeneratedBuildings.MakeBuildingAlwaysOperational(go);
go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
go.AddComponent<ZoneTile>();
go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = Hash.SDBMLower("tiles_" +  tag + "_tops");
BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), tag);
}

static void DoPostConfigureUnderConstruction(GameObject go)
{
go.AddOrGet<KAnimGridTileVisualizer>();
}

static void DoPostConfigureComplete(GameObject go)
{
GeneratedBuildings.RemoveLoopingSounds(go);


}
}
}
*/