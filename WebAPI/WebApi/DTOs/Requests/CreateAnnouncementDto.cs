namespace YourNamespace.DTOs
{
    public class CreateAnnouncementDto<GenericTag>
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public List<GenericTag> Tags { get; set; }//TODO:this is suppose to be a List but I need to fix it Dont forget !!

        public bool IsUrgent { get; set; }
    }
}