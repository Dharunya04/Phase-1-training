public class ReceptionistDT
    {
       public string UserName { get; set; }       
    public string Password { get; set; }      
    public string RoleName { get; set; }        
    public string ReceptionistName { get; set; }  
    public string Email { get; set; }           
    public string PhoneNumber { get; set; } 
    }
public class GetRecep
{
    public int ReceptionistID { get; set; }
    public string ReceptionistName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}
