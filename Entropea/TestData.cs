using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entropea
{
    public static class TestData
    {
        public static string sandstoneStarter = @"biomeNoise: noise/SandstoneStart
temperatureRange: Room
pdWeight: 2
density:
  min: 10
  max: 20
avoidRadius: 20.0
minChildCount: 6
doAvoidPoints: false
dontRelaxChildren: true
sampleBehaviour: PoissonDisk
borderOverride: rocky
biomes:
  - name: biomes/Sedimentary/Basic
    weight: 4
    tags:
      - LightBug
      - PrickleFlower
      - PrickleFlowerSeed
      - PrickleGrass
      - PrickleGrassSeed
      - Hatch
      - HatchBuried  
      - BasicSingleHarvestPlantSeed
      - BasicSingleHarvestPlant
      - BasicForagePlant
      - BasicForagePlantPlanted
  - name: biomes/Sedimentary/Metal_CO2
    weight: 2
    tags:   
      - Hatch
      - HatchBuried
      - BasicForagePlant
      - BasicForagePlantPlanted
  - name: biomes/Sedimentary/Basic_CO2
    weight: 1
    tags:
      - Hatch
      - HatchBuried
      - BasicForagePlant
      - BasicForagePlantPlanted
centralFeature:
  type: features/generic/StartLocation
features:
  - type: features/sedimentary/SmallLake
# - type: features/sedimentary/MetalVacuumBlob
  - type: features/sedimentary/FlatLake
# - type: features/sedimentary/MediumLake  
tags:
  - IgnoreCaveOverride
  - NoGlobalFeatureSpawning
  - StartWorld
zoneType: Sandstone
";
        public static string forestWorld = @"name: STRINGS.WORLDS.FOREST_DEFAULT.NAME
description: STRINGS.WORLDS.FOREST_DEFAULT.DESCRIPTION
spriteName: Asteroid_forest
coordinatePrefix: FRST-A
difficulty: 3
tier: 2

worldsize:
  X: 256
  Y: 384
layoutMethod: PowerTree # Note: We need to add weights to the items in the layer
#                                in order to use this.

# List all the zone files to load
subworldFiles:
  - name: subworlds/forest/ForestStart
  - name: subworlds/forest/ForestMiniWater
    weight: 0.5
  - name: subworlds/forest/ForestMiniOxy
  - name: subworlds/forest/ForestMiniMetal
  - name: subworlds/forest/Forest
  - name: subworlds/jungle/Jungle
  - name: subworlds/frozen/Frozen
  - name: subworlds/ocean/Ocean
  - name: subworlds/rust/Rust
  - name: subworlds/Impenetrable
  - name: subworlds/magma/Bottom
  - name: subworlds/oil/OilPockets
  - name: subworlds/space/Space
  - name: subworlds/space/Surface

startSubworldName: subworlds/forest/ForestStart
startingBaseTemplate: forestBase

globalFeatureTemplates:
  feature_geyser_generic: 12


# Rivers:
#   - water
#   - fatWater
#   - oilygoodness

# When we are converting unknown cells, this will give us the options, processed in this order, the effects are cumlative
unknownCellsAllowedSubworlds: 
  - tagcommand: Default
    command: Replace
    subworldNames:
      - subworlds/forest/ForestStart
  - tagcommand: DistanceFromTag
    tag: AtStart
    minDistance: 1
    maxDistance: 1
    command: Replace
    subworldNames:
      - subworlds/forest/ForestMiniOxy
      - subworlds/forest/ForestMiniWater
      - subworlds/forest/ForestMiniMetal
  - tagcommand: DistanceFromTag
    tag: AtStart
    minDistance: 2
    maxDistance: 2
    command: Replace
    subworldNames:
      - subworlds/jungle/Jungle
      - subworlds/rust/Rust
  - tagcommand: DistanceFromTag
    tag: AtStart
    minDistance: 3
    maxDistance: 3
    command: Replace
    subworldNames:
      - subworlds/jungle/Jungle
      - subworlds/ocean/Ocean
      - subworlds/frozen/Frozen
      - subworlds/rust/Rust
  - tagcommand: DistanceFromTag
    tag: AtStart
    minDistance: 4
    maxDistance: 4
    command: Replace
    subworldNames:
      - subworlds/jungle/Jungle
      - subworlds/ocean/Ocean
      - subworlds/frozen/Frozen
      - subworlds/rust/Rust
  - tagcommand: DistanceFromTag
    tag: AtStart
    minDistance: 5
    maxDistance: 999
    command: Replace
    subworldNames:
      - subworlds/jungle/Jungle
      - subworlds/ocean/Ocean
      - subworlds/rust/Rust
      - subworlds/frozen/Frozen
      - subworlds/forest/ForestStart
  - tagcommand: AtTag
    tag: AtDepths
    command: Replace
    subworldNames:
      - subworlds/magma/Bottom
  - tagcommand: DistanceFromTag
    tag: AtDepths
    minDistance: 1
    maxDistance: 1
    command: Replace
    subworldNames:
      - subworlds/oil/OilPockets
  - tagcommand: AtTag
    tag: AtSurface
    command: Replace
    subworldNames:
      - subworlds/space/Space
  - tagcommand: DistanceFromTag
    tag: AtSurface
    minDistance: 1
    maxDistance: 1
    command: Replace
    subworldNames:
      - subworlds/space/Surface
";
    }
}
