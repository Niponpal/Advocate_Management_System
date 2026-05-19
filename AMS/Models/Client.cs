using AMS.Models;

public class Client
{
public int Id { get; set; }
public string ClientName { get; set; }
public string Email { get; set; }
public string Phone { get; set; }
public string Address { get; set; }
public string NIDNumber { get; set; }

public  ICollection<Case> Cases { get; set; }
}