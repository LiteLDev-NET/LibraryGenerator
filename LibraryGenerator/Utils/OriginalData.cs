using System.Text.Json.Serialization;

namespace LibraryGenerator.Utils;

internal struct OriginalData
{
    [JsonPropertyName("classes")]
    public Dictionary<string, Class> Classes { get; set; }
    public struct Class
    {
        [JsonPropertyName("parent_types")]
        public List<string> ParentTypes { get; set; }
        [JsonPropertyName("public")]
        public List<Item> Public { get; set; }
        [JsonPropertyName("virtual")]
        public List<Item> Virtual { get; set; }
        [JsonPropertyName("vtbl_entry")]
        public List<string> VtblEntry { get; set; }
        [JsonPropertyName("child_types")]
        public List<string> ChildTypes { get; set; }
        [JsonPropertyName("protected")]
        public List<Item> Protected { get; set; }
        [JsonPropertyName("public.static")]
        public List<Item> PublicStatic { get; set; }
        [JsonPropertyName("private.static")]
        public List<Item> PrivateStatic { get; set; }
        public struct Item
        {
            [JsonPropertyName("access_type")]
            public int AccessType { get; set; }
            [JsonPropertyName("fake_symbol")]
            public string FakeSymbol { get; set; }
            [JsonPropertyName("flag_bits")]
            public int FlagBits { get; set; }
            [JsonPropertyName("name")]
            public string Name { get; set; }
            [JsonPropertyName("namespace")]
            public string Namespace { get; set; }
            [JsonPropertyName("params")]
            public List<TypeData> Params { get; set; }
            public struct TypeData
            {
                [JsonPropertyName("kind")]
                public int Kind { get; set; }
                [JsonPropertyName("name")]
                public string Name { get; set; }
            }
            [JsonPropertyName("rva")]
            public ulong RVA { get; set; }
            [JsonPropertyName("storage_class")]
            public int StorageClass { get; set; }
            [JsonPropertyName("symbol")]
            public string Symbol { get; set; }
            [JsonPropertyName("symbol_type")]
            public int SymbolType { get; set; }
            [JsonPropertyName("type")]
            public TypeData Type { get; set; }
        }
    }
    [JsonPropertyName("fn_list")]
    public Dictionary<string, int> FunctionList { get; set; }
    [JsonPropertyName("identifier")]
    public IdentifierData Identifier { get; set; }
    public struct IdentifierData
    {
        [JsonPropertyName("class")]
        public List<string> Class { get; set; }
        [JsonPropertyName("struct")]
        public List<string> Struct { get; set; }
    }
    [JsonPropertyName("sha_256_hash")]
    public Hashs Sha256Hash { get; set; }
    public struct Hashs
    {
        [JsonPropertyName("bedrock_server.exe")]
        public string Exe { get; set; }
        [JsonPropertyName("bedrock_server.pdb")]
        public string Pdb { get; set; }
    }
}
