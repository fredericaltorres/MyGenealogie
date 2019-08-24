namespace MyGenealogie.Console
{
    public interface IPersonDB
    {
        void LoadFromAzureStorageDB();
        void LoadFromLocalDB();
    }
}