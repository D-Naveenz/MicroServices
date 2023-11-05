namespace User_Management.Services
{
    public class Utils
    {
        public static byte[] GuidToByteArray(Guid guid)
        {
            return guid.ToByteArray();
        }

        public static Guid ByteArrayToGuid(byte[] bytes)
        {
            return new Guid(bytes);
        }
    }
}
