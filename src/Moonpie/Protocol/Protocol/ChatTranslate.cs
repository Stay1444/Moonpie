﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Moonpie.Protocol.Protocol;

public class ChatTranslate
{
    public class ChatTranslateClickEvent
    {

        [JsonPropertyName("action"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Action { get; set; }
        [JsonPropertyName("valuie"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Value { get; set; }
    }

    public class ChatTranslateName
    {
        [JsonPropertyName("text"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Text { get; set; }
    }

    public class ChatTranslateContents
    {
        [JsonPropertyName("type"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Type { get; set; }
        [JsonPropertyName("id"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Id { get; set; }
        [JsonPropertyName("name"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ChatTranslateName? Name { get; set; }
    }

    public class ChatTranslateHoverEvent
    {
        [JsonPropertyName("action"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Action { get; set; }
        [JsonPropertyName("contents"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ChatTranslateContents? Contents { get; set; }
    }
    
    public class ChatTranslateWith
    {
        [JsonPropertyName("insertion"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Insertion { get; set; }
        [JsonPropertyName("clickEvent"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ChatTranslateClickEvent? ClickEvent { get; set; }
        [JsonPropertyName("hoverEvent"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ChatTranslateHoverEvent? HoverEvent { get; set; }
        [JsonPropertyName("text"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Text { get; set; }
        [JsonPropertyName("translate"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Translate { get; set; }
        [JsonPropertyName("color"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Color { get; set; }
    }
    
    [JsonPropertyName("translate"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Translate { get; set; }
    [JsonPropertyName("with"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<ChatTranslate> With { get; set; }
    [JsonPropertyName("extra"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<ChatTranslate>? Extra { get; set; }
    [JsonPropertyName("text"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Text { get; set; }
    [JsonPropertyName("color"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Color { get; set; }
    [JsonPropertyName("insertion"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Insertion { get; set; }
    [JsonPropertyName("clickEvent"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ChatTranslateClickEvent? ClickEvent { get; set; }
    [JsonPropertyName("hoverEvent"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ChatTranslateHoverEvent? HoverEvent { get; set; }

}