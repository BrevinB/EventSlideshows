namespace DataAccessLibrary;

public interface IImageData
{
    Task<List<Image>> GetImages(PersonModel person);
    Task SaveImage(Image image);
    Task DeleteImage(Image image);
}

public class ImageData : IImageData
{
    private readonly ISqlDataAccess _db;

    public ImageData(ISqlDataAccess db)
    {
        _db = db;
    }

    public Task<List<Image>> GetImages(PersonModel person)
    {
        string sql = @"SELECT * FROM dbo.Images WHERE PeopleId = @Id";
        return _db.LoadData<Image, dynamic>(sql, person);
    }

    public Task SaveImage(Image image)
    {
        string sql = @"INSERT INTO dbo.Images(PeopleId, Data) VALUES(@PeopleId, @Data)";
        return _db.SaveData(sql, image);
    }

    public Task DeleteImage(Image image)
    {
        string sql = @"DELETE FROM dbo.Images WHERE Id = @Id";
        return _db.SaveData(sql, image);
    }
}