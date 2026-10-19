namespace FindFix.Models;
public class SecurityQuestion
{
    public string Text {get; set;}= "";
    public List<string> Options {get; set;}= new();
    public string CorrectAnswer {get; set;}= string.Empty;

}