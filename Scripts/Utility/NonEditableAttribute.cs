using UnityEngine;

namespace Clickbait.Utilities
{
    /// <summary>
    /// Needs to be in the Scripts folder, not Editor, so that other scripts can access it
    /// </summary>
    
    public class NonEditableAttribute : PropertyAttribute { }
}