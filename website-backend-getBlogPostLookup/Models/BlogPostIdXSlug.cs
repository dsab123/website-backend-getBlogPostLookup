using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace website_backend_getBlogPostLookup.Models
{
  public class BlogPostIdXSlug
  {
    [JsonProperty(PropertyName = "id")]
    public int Id { get; set; }
    [JsonProperty(PropertyName = "slug")]
    public string Slug { get; set; }

    public BlogPostIdXSlug(int id, string slug)
    {
      Id = id;
      Slug = slug;
    }
  }
}
