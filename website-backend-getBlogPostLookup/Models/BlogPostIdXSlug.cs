using Newtonsoft.Json;

namespace website_backend_getBlogPostLookup.Models
{
  public class BlogPostIdXSlug
  {
    [JsonProperty(PropertyName = "id")]
    public int Id { get; set; }
    [JsonProperty(PropertyName = "slug")]
    public string Slug { get; set; }

    [JsonConstructor]
    public BlogPostIdXSlug(int id, string slug)
    {
      Id = id;
      Slug = slug;
    }
  }
}
