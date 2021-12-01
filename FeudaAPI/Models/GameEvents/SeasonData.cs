namespace FeudaAPI.GameEvents
{
    public class SeasonData
    {

        //Additional modifier changes the output of every production tile by this amount
        //Direct modifier hardcodes the income per tile to a specific amount
        //PerSerf modifier applies the specified amount of resource change to the income for every serf
        public int additionalFoodIncomeModifier = 0;
        public int? directFoodModifier = null;
        public int perSerfFoodModifier = -1;

        public int additionalWoodIncomeModifier = 0;
        public int? directWoodModifier = null;
        public int perSerfWoodModifier = 0;

        public int additionalOreIncomeModifier = 0;
        public int? directOreModifier = null;
        public int perSerfOreModifier = 0;

        public SeasonData(int additionalFoodIncomeModifier, int? directFoodModifier, int perSerfFoodModifier,
            int additionalWoodIncomeModifier, int? directWoodModifier, int perSerfWoodModifier,
            int additionalOreIncomeModifier, int? directOreModifier, int perSerfOreModifier)
        {
            this.additionalFoodIncomeModifier = additionalFoodIncomeModifier;
            this.directFoodModifier = directFoodModifier;
            this.perSerfFoodModifier = perSerfFoodModifier;
            this.additionalWoodIncomeModifier = additionalWoodIncomeModifier;
            this.directWoodModifier = directWoodModifier;
            this.perSerfWoodModifier = perSerfWoodModifier;
            this.additionalOreIncomeModifier = additionalOreIncomeModifier;
            this.directOreModifier = directOreModifier;
            this.perSerfOreModifier = perSerfOreModifier;
        }
    }
}
