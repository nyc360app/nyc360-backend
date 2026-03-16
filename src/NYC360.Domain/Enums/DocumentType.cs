namespace NYC360.Domain.Enums;

public enum DocumentType
{
    // --- Identity Verification ---
    GovernmentId = 1,          // Driver's License / Passport (For "New Yorker" Identity)
    UtilityBill = 2,           // For Residency Verification
    OrganizationCharter = 3,   // For Non-Profits/Community Groups
    BusinessLicense = 4,       // For For-Profit Businesses/Venues

    // --- Professional & Academic (The "Educator" Scenario) ---
    ProfessionalLicense = 5,   // Medical License (Doctor), Bar Association (Legal)
    EmployeeIdCard = 6,        // School ID, Corporate Badge
    AcademicDegree = 7,        // Diploma, Transcript
    Certification = 8,         // Specialized training (e.g., Tech Cert, Yoga Cert)
    
    // --- Creative & Media (Culture/TV Divisions) ---
    PortfolioLink = 9,         // Link to work (for Artists/Creators)
    PressCredential = 10,      // For Journalism/News Tag
    ContractAgreement = 11,    // Proof of engagement with an NYC Institution
    
    // --- Miscellaneous ---
    LetterOfRecommendation = 12, // From Community Leaders
    Other = 99
}