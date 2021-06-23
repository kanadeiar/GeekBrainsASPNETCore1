namespace WebStore.Dal.Interfaces
{
    public interface IWebStoreDataInit
    {
        /// <summary> Пересоздание базы данных </summary>
        IWebStoreDataInit RecreateDatabase();

        /// <summary> Заполнение начальными данными </summary>
        IWebStoreDataInit InitData();

    }
}