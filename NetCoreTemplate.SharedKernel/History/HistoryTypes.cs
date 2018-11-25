namespace NetCoreTemplate.SharedKernel.History
{
    public static class HistoryTypes
    {
        public const string ResetPassword = "admin.resetpassword";
        public const string ValidToken = "admin.validtoken";
        public const string InvalidToken = "admin.invalidtoken";
        public const string ExpiredToken = "admin.expiredtoken";
        public const string PasswordChanged = "admin.passwordchanged";
        public const string Deactivated = "admin.deactivated";
        public const string SignedIn = "admin.signedin";
        public const string WrongLoginSignIn = "admin.wronglogin";
        public const string TryToSignIn = "admin.invalidIP";
        public const string EditRelationship = "Relationship.edit";
        public const string AddRelationship = "Relationship.add";
        public const string EditStorageBoxCapacity = "StorageBoxCapacity.edit";
        public const string AddStorageBoxCapacity = "StorageBoxCapacity.add";
        public const string EditStorageBox = "StorageBox.edit";
    }
}
