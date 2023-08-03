using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace OptechX.Library.Apps.UpdateTool.Constants;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ApplicationCategory
{
    [EnumMember(Value = "Productivity")]
    PRODUCTIVITY,
    [EnumMember(Value = "Microsoft")]
    MICROSOFT,
    [EnumMember(Value = "Utility")]
    UTILITY,
    [EnumMember(Value = "Developer Tools")]
    DEVELOPERTOOLS,
    [EnumMember(Value = "Games")]
    GAMES,
    [EnumMember(Value = "Photo & Design")]
    PHOTODESIGN,
    [EnumMember(Value = "Entertainment")]
    ENTERTAINMENT,
    [EnumMember(Value = "Security")]
    SECURITY,
    [EnumMember(Value = "Education")]
    EDUCATION,
    [EnumMember(Value = "Internet")]
    INTERNET,
    [EnumMember(Value = "Lifestyle")]
    LIFESTYLE,
    Default
}