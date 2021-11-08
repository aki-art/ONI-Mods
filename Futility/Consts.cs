namespace FUtility
{
    public class Consts
    {
        public static class BUILD_CATEGORY
        {
            ///<summary>Base</summary>
            public const string BASE = "Base";

            ///<summary>Oxygen</summary>
            public const string OXYGEN = "Oxygen";

            ///<summary>Power</summary>
            public const string POWER = "Power";

            ///<summary>Food</summary>
            public const string FOOD = "Food";

            ///<summary>Plumbing</summary>
            public const string PLUMBING = "Plumbing";

            ///<summary>Ventilation</summary>
            public const string HVAC = "HVAC";

            ///<summary>Refinement</summary>
            public const string REFINING = "Refining";

            ///<summary>Medicine</summary>
            public const string MEDICAL = "Medical";

            ///<summary>Furniture</summary>
            public const string FURNITURE = "Furniture";

            ///<summary>Stations</summary>
            public const string EQUIPMENT = "Equipment";

            ///<summary>Utilities</summary>
            public const string UTILITIES = "Utilities";

            ///<summary>Automation</summary>
            public const string AUTOMATION = "Automation";

            ///<summary>Shipping</summary>
            public const string CONVEYANCE = "Conveyance";

            ///<summary>Rocketry</summary>
            public const string ROCKETRY = "Rocketry";

            ///<summary>Radiation</summary>
            public const string HEP = "HEP";
        }

        // categorization referenced from Cailib: https://github.com/Cairath/ONI-Mods/blob/master/src/CaiLib/Utils/GameStrings.cs
        public static class TECH
        {
            public static class FOOD
            {
                ///<summary>Basic Farming</summary>
                public const string FARMING_TECH = "FarmingTech";

                ///<summary>Meal Preparation</summary>
                public const string FINE_DINING = "FineDining";

                ///<summary>Gourmet Meal Preparation</summary>
                public const string FINER_DINING = "FinerDining";

                ///<summary>Food Repurposing</summary>
                public const string FOOD_REPURPOSING = "FoodRepurposing";

                ///<summary>Agriculture</summary>
                public const string AGRICULTURE = "Agriculture";

                ///<summary>Ranching</summary>
                public const string RANCHING = "Ranching";

                ///<summary>Animal Control</summary>
                public const string ANIMAL_CONTROL = "AnimalControl";

                ///<summary>Bioengineering</summary>
                public const string BIOENGINEERING = "Bioengineering";
            }

            public static class POWER
            {
                ///<summary>Power Regulation</summary>
                public const string POWER_REGULATION = "PowerRegulation";

                ///<summary>Internal Combustion</summary>
                public const string COMBUSTION = "Combustion";

                ///<summary>Fossil Fuels</summary>
                public const string IMPROVED_COMBUSTION = "ImprovedCombustion";

                ///<summary>Sound Amplifiers</summary>
                public const string ACOUSTICS = "Acoustics";

                ///<summary>Advanced Power Regulation</summary>
                public const string ADVANCED_POWER_REGULATION = "AdvancedPowerRegulation";

                ///<summary>Plastic Manufacturing</summary>
                public const string PLASTICS = "Plastics";

                ///<summary>Low-Resistance Conductors</summary>
                public const string PRETTY_GOOD_CONDUCTORS = "PrettyGoodConductors";

                ///<summary>Valve Miniaturization</summary>
                public const string VALVE_MINIATURIZATION = "ValveMiniaturization";

                ///<summary>Renewable Energy</summary>
                public const string RENEWABLE_ENERGY = "RenewableEnergy";

                ///<summary>Space Power</summary>
                public const string SPACE_POWER = "SpacePower";

                ///<summary>Hydrocarbon Propulsion</summary>
                public const string HYDRO_CARBON_PROPULSION = "HydrocarbonPropulsion";

                ///<summary>Improved Hydrocarbon Propulsion</summary>
                public const string BETTER_HYDRO_CARBON_PROPULSION = "BetterHydroCarbonPropulsion";

                ///<summary>Advanced Combustion</summary>
                public const string SPACE_COMBUSTION = "SpaceCombustion";
            }

            public static class SOLIDS
            {
                ///<summary>Brute-Force Refinement</summary>
                public const string BASIC_REFINEMENT = "BasicRefinement";

                ///<summary>Refined Renovations</summary>
                public const string REFINED_OBJECTS = "RefinedObjects";

                ///<summary>Smart Storage</summary>
                public const string SMART_STORAGE = "SmartStorage";

                ///<summary>Smelting</summary>
                public const string SMELTING = "Smelting";

                ///<summary>Solid Transport</summary>
                public const string SOLID_TRANSPORT = "SolidTransport";

                ///<summary>Superheated Forging</summary>
                public const string HIGH_TEMP_FORGING = "HighTempForging";

                ///<summary>Pressurized Forging</summary>
                public const string HIGH_PRESSURE_FORGING = "HighPressureForging";

                ///<summary>Solid Control</summary>
                public const string SOLID_SPACE = "SolidSpace";

                ///<summary>Solid Management</summary>
                public const string SOLID_MANAGEMENT = "SolidManagement";

                ///<summary>High Velocity Transport</summary>
                public const string HIGH_VELOCITY_TRANSPORT = "HighVelocityTransport";

                ///<summary>High Velocity Destruction</summary>
                public const string HIGH_VELOCITY_DESTRUCTION = "HighVelocityDestruction";

            }

            public static class COLONY_DEVELOPMENT
            {
                ///<summary>Employment</summary>
                public const string JOBS = "Jobs";

                ///<summary>Advanced Research</summary>
                public const string ADVANCED_RESEARCH = "AdvancedResearch";

                ///<summary>Radiation Refinement</summary>
                public const string NUCLEAR_REFINEMENT = "NuclearRefinement";

                ///<summary>Cryofuel Propulsion</summary>
                public const string CRYO_FUEL_PROPULSION = "CryoFuelPropulsion";

                ///<summary>Space Program</summary>
                public const string SPACE_PROGRAM = "SpaceProgram";

                ///<summary>Crash Plan</summary>
                public const string CRASH_PLAN = "CrashPlan";

                ///<summary>Durable Life Support</summary>
                public const string DURABLE_LIFE_SUPPORT = "DurableLifeSupport";

                ///<summary>Materials Science Research</summary>
                public const string NUCLEAR_RESEARCH = "NuclearResearch";

                ///<summary>More Materials Science Research</summary>
                public const string ADVANCED_NUCLEAR_RESEARCH = "AdvancedNuclearResearch";

                ///<summary>Radbolt Propulsion</summary>
                public const string NUCLEAR_PROPULSION = "NuclearPropulsion";

                ///<summary>Notification Systems</summary>
                public const string NOTIFICATION_SYSTEMS = "NotificationSystems";

                ///<summary>Artificial Friends</summary>
                public const string ARTIFICIAL_FRIENDS = "ArtificialFriends";

                ///<summary>Robotic Tools</summary>
                public const string ROBOTIC_TOOLS = "RoboticTools";
            }

            public static class MEDICINE
            {
                ///<summary>Pharmacology</summary>
                public const string MEDICINEI = "MedicineI";

                ///<summary>Medical Equipment</summary>
                public const string MEDICINEII = "MedicineII";

                ///<summary>Pathogen Diagnostics</summary>
                public const string MEDICINEIII = "MedicineIII";

                ///<summary>Micro-Targeted Medicine</summary>
                public const string MEDICINEIV = "MedicineIV";

                ///<summary>Radiation Protection</summary>
                public const string RADIATION_PROTECTION = "RadiationProtection";
            }

            public static class LIQUIDS
            {
                ///<summary>Plumbing</summary>
                public const string LIQUID_PIPING = "LiquidPiping";

                ///<summary>Air Systems</summary>
                public const string IMPROVED_OXYGEN = "ImprovedOxygen";

                ///<summary>Sanitation</summary>
                public const string SANITATION_SCIENCES = "SanitationSciences";

                ///<summary>Advanced Sanitation</summary>
                public const string ADVANCED_SANITATION = "AdvancedSanitation";

                ///<summary>Filtration</summary>
                public const string ADVANCED_FILTRATION = "AdvancedFiltration";

                ///<summary>Liquid-Based Refinement Processes</summary>
                public const string LIQUID_FILTERING = "LiquidFiltering";

                ///<summary>Distillation</summary>
                public const string DISTILLATION = "Distillation";

                ///<summary>Improved Plumbing</summary>
                public const string IMPROVED_LIQUID_PIPING = "ImprovedLiquidPiping";

                ///<summary>Liquid Tuning</summary>
                public const string LIQUID_TEMPERATURE = "LiquidTemperature";

                ///<summary>Advanced Caffeination</summary>
                public const string PRECISION_PLUMBING = "PrecisionPlumbing";

                ///<summary>Flow Redirection</summary>
                public const string FLOW_REDIRECTION = "FlowRedirection";

                ///<summary>Liquid Distribution</summary>
                public const string LIQUID_DISTRIBUTION = "LiquidDistribution";

                ///<summary>Jetpacks</summary>
                public const string JETPACKS = "Jetpacks";
            }

            public static class GASES
            {
                ///<summary>Ventilation</summary>
                public const string GAS_PIPING = "GasPiping";

                ///<summary>Pressure Management</summary>
                public const string PRESSURE_MANAGEMENT = "PressureManagement";

                ///<summary>Temperature Modulation</summary>
                public const string TEMPERATURE_MODULATION = "TemperatureModulation";

                ///<summary>Decontamination</summary>
                public const string DIRECTED_AIR_STREAMS = "DirectedAirStreams";

                ///<summary>Improved Ventilation</summary>
                public const string IMPROVED_GAS_PIPING = "ImprovedGasPiping";

                ///<summary>HVAC</summary>
                public const string HVAC = "HVAC";

                ///<summary>Catalytics</summary>
                public const string CATALYTICS = "Catalytics";

                ///<summary>Portable Gases</summary>
                public const string PORTABLE_GASSES = "PortableGasses";

                ///<summary>Advanced Gas Flow</summary>
                public const string SPACEGAS = "SpaceGas";

                ///<summary>Gas Distribution</summary>
                public const string GAS_DISTRIBUTION = "GasDistribution";
            }

            public static class EXOSUITS
            {
                ///<summary>Hazard Protection</summary>
                public const string SUITS = "Suits";

                ///<summary>Transit Tubes</summary>
                public const string TRAVEL_TUBES = "TravelTubes";
            }

            public static class DECOR
            {
                ///<summary>Interior Decor</summary>
                public const string INTERIOR_DECOR = "InteriorDecor";

                ///<summary>Artistic Expression</summary>
                public const string ARTISTRY = "Artistry";

                ///<summary>Textile Production</summary>
                public const string CLOTHING = "Clothing";

                ///<summary>Fine Art</summary>
                public const string FINEART = "FineArt";

                ///<summary>Home Luxuries</summary>
                public const string LUXURY = "Luxury";

                ///<summary>High Culture</summary>
                public const string REFRACTIVE_DECOR = "RefractiveDecor";

                ///<summary>Glass Blowing</summary>
                public const string GLASS_FURNISHINGS = "GlassFurnishings";

                ///<summary>Renaissance Art</summary>
                public const string RENAISSANCE_ART = "RenaissanceArt";

                ///<summary>Environmental Appreciation</summary>
                public const string ENVIRONMENT_ALAPPRECIATION = "EnvironmentalAppreciation";

                ///<summary>New Media</summary>
                public const string SCREENS = "Screens";

                ///<summary>Monuments</summary>
                public const string MONUMENTS = "Monuments";
            }

            public static class COMPUTERS
            {
                ///<summary>Smart Home</summary>
                public const string LOGIC_CONTROL = "LogicControl";

                ///<summary>Generic Sensors</summary>
                public const string GENERIC_SENSORS = "GenericSensors";

                ///<summary>Advanced Automation</summary>
                public const string LOGIC_CIRCUITS = "LogicCircuits";

                ///<summary>Computing</summary>
                public const string DUPE_TRAFFIC_CONTROL = "DupeTrafficControl";

                ///<summary>Parallel Automation</summary>
                public const string PARALLEL_AUTOMATION = "ParallelAutomation";

                ///<summary>Multiplexing</summary>
                public const string MULTIPLEXING = "Multiplexing";

                ///<summary>Sensitive Microimaging</summary>
                public const string ADVANCED_SCANNERS = "AdvancedScanners";
            }

            public static class ROCKETS
            {
                ///<summary>Celestial Detection</summary>
                public const string SKY_DETECTORS = "SkyDetectors";

                ///<summary>Introductory Rocketry</summary>
                public const string BASIC_ROCKETRY = "BasicRocketry";

                ///<summary>Solid Fuel Combustion</summary>
                public const string ENGINESI = "EnginesI";

                ///<summary>Solid Cargo</summary>
                public const string CARGOI = "CargoI";

                ///<summary>Hydrocarbon Combustion</summary>
                public const string ENGINESII = "EnginesII";

                ///<summary>Liquid and Gas Cargo</summary>
                public const string CARGOII = "CargoII";

                ///<summary>Cryofuel Combustion</summary>
                public const string ENGINESIII = "EnginesIII";

                ///<summary>Unique Cargo</summary>
                public const string CARGOIII = "CargoIII";

                ///<summary>Advanced Resource Extraction</summary>
                public const string ADVANCED_RESOURCE_EXTRACTION = "AdvancedResourceExtraction";
            }
        }
    }
}
