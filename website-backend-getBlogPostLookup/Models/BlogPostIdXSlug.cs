namespace website_backend_getBlogPostLookup.Models
{
  public class BlogPostIdXSlug
  {
    public int Id { get; set; }
    public string Slug { get; set; }

    public BlogPostIdXSlug(int id, string slug)
    {
      Id = id;
      Slug = slug;
    }
  }
}
