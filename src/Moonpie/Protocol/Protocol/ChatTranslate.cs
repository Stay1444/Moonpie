#region Copyright
// Moonpie
// 
// Copyright (c) 2022 Stay
// 
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using System.Collections.Generic;
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