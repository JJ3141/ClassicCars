namespace ClassicCars
{
	public static class EntityValidations
	{
        //User
        public const int MaxLenghtUsername = 50;
        public const int MinLenghtUsername = 2;

        //Password
        public const int MaxPasswordLength = 25;
        public const int MinPasswordLength = 2;

        //Name
        public const int MaxLenghtName = 100;
        public const int MinLenghtName = 2;

        //Email
        public const int MaxEmailLenght = 75;
        public const int MinEmailLenght = 2;

        // Brand
        public const int CarBrandMaxLength = 50;
        public const int CarBrandMinLength = 2;

        // Model
        public const int CarModelMaxLength = 50;
        public const int CarModelMinLength = 1;

        // Engine
        public const int EngineTypeMaxLength = 50;
        public const int EngineTypeMinLength = 2;

        // Condition
        public const int ConditionMaxLength = 50;
        public const int ConditionMinLength = 2;

        // Transmission
        public const int TransmissionMaxLength = 30;
        public const int TransmissionMinLength = 2;

        // Description
        public const int DescriptionMaxLength = 1000;
        public const int DescriptionMinLength = 5;

        // Horsepower
        public const int MinHorsepower = 1;
        public const int MaxHorsepower = 3000;

        // Price
        public const decimal MinPrice = 1;
        public const decimal MaxPrice = 100000000;

        public const int ServiceDescriptionMaxLength = 1000;
        public const int ServiceDescriptionMinLength = 5;

        // Mileage
        public const double MinMileage = 0;
        public const double MaxMileage = 2000000;

        public const int ReviewCommentMaxLength = 350;
        public const int ReviewCommentMinLength = 2;

        public const int WarrantyProviderMaxLength = 200;
        public const int WarrantyProviderMinLength = 1;

        public const int WarrantyNotesMaxLength = 200;
        public const int WarrantyNotesMinLength = 2;

        public const int InsuranceProviderMaxLength = 100;
        public const int InsuranceProviderMinLength = 2;


        public const int InsuranceNotesMaxLength = 500;
        public const int InsuranceNotesMinLength = 2;
    }
}


