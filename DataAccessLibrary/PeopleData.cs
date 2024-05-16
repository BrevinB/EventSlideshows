namespace DataAccessLibrary;

public interface IPeopleData
{
    Task<List<PersonModel>> GetPeople();
    Task InsertPerson(PersonModel person);
    Task RemovePerson(PersonModel person);
    
}

public class PeopleData : IPeopleData
{
    private readonly ISqlDataAccess _db;

    public PeopleData(ISqlDataAccess db)
    {
        _db = db;
    }
    
    public Task<List<PersonModel>> GetPeople()
    {
        string sql = "select * from dbo.People";
        return _db.LoadData<PersonModel, dynamic>(sql, new { });
    }

    public Task InsertPerson(PersonModel person)
    {
        string sql = @"INSERT INTO dbo.People (FName, LName)
                        VALUES (@FName, @LName);";
        
        return _db.SaveData(sql, person);
    }

    public Task RemovePerson(PersonModel person)
    {
        string sql = @"DELETE FROM dbo.People WHERE ID = @ID";

        return _db.SaveData(sql, person);
    }
    
}