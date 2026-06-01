namespace apbd_tut_9.Models;

public partial class Assignment
{
    public bool IsOverdute(DateTime now)
    {
        return DueDate < now;
    }
}