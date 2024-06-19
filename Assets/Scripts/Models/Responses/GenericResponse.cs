public class GenericResponse
{
    public bool IsSucceeded { get; set; }

    public string ErrorMessage { get; set; }

    public string SuccessMessage { get; set; }

    public GenericResponse(bool isSucceeded, string errorMessage, string successMessage = null)
	{
        IsSucceeded = isSucceeded;
        ErrorMessage = errorMessage;
        SuccessMessage = successMessage;
	}

    public static GenericResponse SetOk(string successMessage = null)
	{
        return new GenericResponse(true, null, successMessage);
    }

    public static GenericResponse SetFail(string errorMessage)
    {
        return new GenericResponse(false, errorMessage);
    }
}

public class GenericResponse<T>
{
    public bool IsSucceeded { get; set; }

    public string ErrorMessage { get; set; }

    public string SuccessMessage { get; set; }

    public T Value { get; set; }

    public GenericResponse(bool isSucceeded, string errorMessage, T value, string successMessage = null)
    {
        IsSucceeded = isSucceeded;
        ErrorMessage = errorMessage;
        Value = value;
        SuccessMessage = successMessage;
    }

    public static GenericResponse<T> SetOk(T value, string successMessage = null)
    {
        return new GenericResponse<T>(true, null, value, successMessage);
    }

    public static GenericResponse<T> SetFail(string errorMessage)
    {
        return new GenericResponse<T>(false, errorMessage, default(T));
    }
}