using System.Text.RegularExpressions;

internal static class RegexHelper
{
    public static readonly Regex std_basic_string_regex =
        new(
            @"class std::basic_string<(?<char_type>.*), struct std::char_traits<(\k<char_type>)>, class std::allocator<(\k<char_type>)>>");

    public static readonly Regex std_vector_regex =
        new(@"class std::vector<(?<class_type>.*), class std::allocator<(\k<class_type>)>>");

    public static readonly Regex std_optional_regex = new(@"class std::optional<(?<class_type>.*)>");

    public static readonly Regex std_unordered_map_regex = new(
        @"class std::unordered_map<(?<class_type_1>.*), (?<class_type_2>.*), struct std::hash<(\k<class_type_1>)>, struct std::equal_to<(\k<class_type_1>)>, class std::allocator<struct std::pair<(\k<class_type_1>) const, (\k<class_type_2>)>>>");

    public static readonly Regex std_function_regex = new(@"class std::function<(?<function_type>.*)>");

    public static readonly Regex gsl_not_null_regex = new(@"class gsl::not_null<(?<class_type>.*)>");

    public static readonly Regex gsl_basic_string_span_regex =
        new(@"class gsl::basic_string_span<(?<char_type>.*), (?<value>.*)>");

}
