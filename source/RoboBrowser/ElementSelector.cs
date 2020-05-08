namespace RoboBrowser
{
    public class ElementSelector
    {
        private string _function = "getElementById";

        public string ElementId { get; }
        public string ExtraQuery { get; }

        public ElementSelector ChildCount => this + ".childElementCount";
        public ElementSelector Disabled => this + ".disabled";
        public ElementSelector Display => this + ".display";
        public ElementSelector Href => this + ".href";
        public ElementSelector Id => this + ".id";
        public ElementSelector InnerHtml => this + ".innerHTML";
        public ElementSelector InnerText => this + ".innerText";
        public ElementSelector Length => this + ".length";
        public ElementSelector Options => this + ".options";
        public ElementSelector ParentElement => this + ".parentElement";
        public ElementSelector Style => this + ".style";
        public ElementSelector TextContent => this + ".textContent";
        public ElementSelector Value => this + ".value";
        public ElementSelector Width => this + ".width";

        private ElementSelector(string id, string extraQuery, string function) =>
            (ElementId, ExtraQuery, _function) = (id, extraQuery, function);

        public ElementSelector(string id, string extraQuery = null) =>
            (ElementId, ExtraQuery) = (id, extraQuery);

        public static ElementSelector GetElementById(string id) => new ElementSelector(id);
        public static ElementSelector GetElementsByClassName(string className) =>
            new ElementSelector(className, null, "getElementsByClassName");
        public static ElementSelector GetElementsByTagName(string tag) =>
            new ElementSelector(tag, null, "getElementsByTagName");

        public static ElementSelector operator +(ElementSelector element, string query) =>
            new ElementSelector(element.ElementId, $"{element.ExtraQuery}{query}", element._function);

        public static implicit operator ElementSelector(string id) => new ElementSelector(id);

        public static implicit operator ElementSelector((string id, string query) tuple) => new ElementSelector(tuple.id, tuple.query);

        public static implicit operator string(ElementSelector id) => id.ToString();

        public ElementSelector this[int index] => this + $"[{index}]";

        public ElementSelector Child(int no) => this + $".children.item({no})";
        public ElementSelector QuerySelector(string query) => this + $".querySelector('{query}')";

        public override string ToString() => $"document.{_function}('{ElementId}'){ExtraQuery}";
    }
}
