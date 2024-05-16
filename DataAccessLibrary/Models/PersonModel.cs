namespace DataAccessLibrary;

public class PersonModel
{
    public int Id { get; set; }
    public string FName { get; set; } = String.Empty;
    public string LName { get; set; } = String.Empty;
    public List<Image> Images { get; set; } = new List<Image>();
}