namespace MaidService.Library.DbModels;

public class PublicUser
{

    public string Pfp;
    public string AuthId { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string FullName => $"{FirstName} {Surname}";
    public int Id { get; set; }
    public string PhoneNumber { get; set; }
    public string Surname { get; set; }

    public string ProfilePictureUrl { get; set; }
}