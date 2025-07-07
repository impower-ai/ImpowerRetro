namespace ImpowerRetro.Components.Model;

public class ServiceResult
{
	public bool Successful { get; set; }
	public string Message { get; set; } = string.Empty;

	public static ServiceResult Success(string message = "") => new() { Successful = true, Message = message };
	public static ServiceResult Failure(string message) => new() { Successful = false, Message = message };

	public static DataServiceResult<T> Success<T>(T data, string message = "") => new() { Successful = true, Data = data, Message = message };
	public static DataServiceResult<T> Failure<T>(string message) => new() { Successful = false, Data = default, Message = message };
}

public class DataServiceResult<T> : ServiceResult
{
	public T Data { get; set; }

	public static DataServiceResult<T> Success(T data, string message = "") => new() { Successful = true, Data = data, Message = message };
	public new static DataServiceResult<T> Failure(string message) => new() { Successful = false, Data = default, Message = message };
}
