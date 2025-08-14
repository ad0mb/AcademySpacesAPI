namespace YourNamespace.DTOs
{
    public class CreateAnnouncementDto
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Tags { get; set; }//TODO:this is suppose to be a List but I need to fix it Dont forget !!

        public bool IsUrgent { get; set; }
    }
}