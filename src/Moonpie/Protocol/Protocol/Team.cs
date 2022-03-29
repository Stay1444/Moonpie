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

namespace Moonpie.Protocol.Protocol;

public class Team
{
    public enum TeamCollisionRules
    {
        Always,
        PushEnemies,
        PushMates,
        Never
    }

    public enum NameTagVisibilities
    {
        Always,
        HideForEnemies,
        HideForMates,
        Never
    }
    
    public string Name { get; set; } = "";
    public ChatComponent DisplayName = ChatComponent.Empty;
    public ChatComponent Prefix = ChatComponent.Empty;
    public ChatComponent Suffix = ChatComponent.Empty;
    public bool FriendlyFire = true;
    public bool CanSeeInvisibleTeamMembers = true;
    public TeamCollisionRules CollisionRule { get; set; } = TeamCollisionRules.Always;
    public NameTagVisibilities NameTagVisibility { get; set; } = NameTagVisibilities.Always;
    public string? FormattingCode { get; set; }
    public string[] Members { get; set; } = new string[0];
}