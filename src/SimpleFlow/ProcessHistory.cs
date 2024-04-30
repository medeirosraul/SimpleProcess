namespace SimpleFlow
{
    public class ProcessHistory
    {
        public required string Name { get; set; }
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public string? Error { get; set; }

        public static ProcessHistory Success(string name, string? message = null) 
            => new ProcessHistory { Name = name, Succeeded = true, Message = message };

        public static ProcessHistory Fail(string name, string error)
            => new ProcessHistory { Name = name, Succeeded = false, Error = error };
    }
}
