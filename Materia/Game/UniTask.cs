using System.Runtime.InteropServices;
using ECGen.Generated;
using ECGen.Generated.Cysharp.Threading.Tasks;

namespace Materia.Game;

public enum UniTaskStatus
{
    Pending,
    Succeeded,
    Faulted,
    Canceled
}

internal class UniTask : IDisposable
{
    private readonly Il2CppObject<ECGen.Generated.Cysharp.Threading.Tasks.UniTask> o;

    public unsafe UniTask(ECGen.Generated.Cysharp.Threading.Tasks.UniTask* ptr) => o = new Il2CppObject<ECGen.Generated.Cysharp.Threading.Tasks.UniTask>(ptr);

    public async Task Await()
    {
        while (GetStatus() == UniTaskStatus.Pending)
            await Task.Yield();
    }

    public unsafe UniTaskStatus GetStatus() => o.Ptr->GetStatus();

    public void Dispose()
    {
        o.Dispose();
        GC.SuppressFinalize(this);
    }
}

internal class UniTask<T> : IDisposable where T : unmanaged
{
    private readonly Il2CppObject<ECGen.Generated.Cysharp.Threading.Tasks.UniTask<T>> o;

    public unsafe UniTask(ECGen.Generated.Cysharp.Threading.Tasks.UniTask<T>* ptr) => o = new Il2CppObject<ECGen.Generated.Cysharp.Threading.Tasks.UniTask<T>>(ptr);

    public async Task<T> GetTask()
    {
        while (true)
        {
            switch (GetStatus())
            {
                case UniTaskStatus.Succeeded:
                    return GetResult();
                case UniTaskStatus.Faulted:
                    throw new ApplicationException("UniTask threw an unhandled exception");
                case UniTaskStatus.Canceled:
                    throw new TaskCanceledException("UniTask was canceled");
                case UniTaskStatus.Pending:
                    await Task.Yield();
                    break;
            }
        }
    }

    public async Task<Ptr<T>> GetTaskPtr()
    {
        while (true)
        {
            switch (GetStatus())
            {
                case UniTaskStatus.Succeeded:
                    return GetResultPtr();
                case UniTaskStatus.Faulted:
                    throw new ApplicationException("UniTask threw an unhandled exception");
                case UniTaskStatus.Canceled:
                    throw new TaskCanceledException("UniTask was canceled");
                case UniTaskStatus.Pending:
                    await Task.Yield();
                    break;
            }
        }
    }

    public unsafe UniTaskStatus GetStatus() => o.Ptr->GetStatus();
    public unsafe T GetResult() => o.Ptr->GetResult();
    public unsafe Ptr<T> GetResultPtr() => new(o.Ptr->GetResultPtr());

    public void Dispose()
    {
        o.Dispose();
        GC.SuppressFinalize(this);
    }
}

internal unsafe class UniTaskContinuable(ECGen.Generated.Cysharp.Threading.Tasks.UniTask* uniTask, object a) : IDisposable
{
    private readonly Il2CppObject<ECGen.Generated.Cysharp.Threading.Tasks.UniTask> o = new(uniTask);

    public bool CheckInvoke()
    {
        var type = a.GetType();
        var argType = type.IsGenericType ? type.GenericTypeArguments.First() : null;
        switch (GetStatus(argType))
        {
            case UniTaskStatus.Succeeded:
                if (argType != null)
                {
                    if (argType.IsGenericType) // Ptr<T>
                    {
                        var ptr = Activator.CreateInstance(argType)!;
                        argType.GetField(nameof(Ptr<nint>.ptr))?.SetValue(ptr, GetResultPtr());
                        type.GetMethod("Invoke")!.Invoke(a, [ ptr ]);
                    }
                    else // T
                    {
                        type.GetMethod("Invoke")!.Invoke(a, [ GetResult(argType) ]);
                    }
                }
                else
                {
                    type.GetMethod("Invoke")!.Invoke(a, null);
                }
                Dispose();
                return true;
            case UniTaskStatus.Faulted:
                Dispose();
                throw new ApplicationException("UniTask threw an unhandled exception");
            case UniTaskStatus.Canceled:
                Dispose();
                throw new TaskCanceledException("UniTask was canceled");
            default:
                return false;
        }
    }

    private short GetToken(Type? resultType)
    {
        if (resultType == null) return o.Ptr->token;
        var size = resultType == typeof(bool)
            ? 1
            : resultType == typeof(char)
                ? 2
                : Marshal.SizeOf(resultType);
        var remainder = size % 2;
        if (remainder > 0)
            size += 2 - remainder;
        var offset = 8 + size;
        return *(short*)((nint)o.Ptr + offset);
    }

    private UniTaskStatus GetStatus(Type? resultType)
    {
        if (o.Ptr->source == null) return UniTaskStatus.Succeeded;
        var invokeData = GameInterop.GetVirtualFunctionByInterface<IUniTaskSource>(o.Ptr->source, 0);
        return ((delegate* unmanaged<IUniTaskSource*, short, Il2CppMethodInfo*, UniTaskStatus>)invokeData->methodPtr)(o.Ptr->source, GetToken(resultType), invokeData->method);
    }

    private object GetResult(Type resultType)
    {
        if (o.Ptr->source == null) return Marshal.PtrToStructure((nint)(&o.Ptr->source) + 8, resultType)!;
        var invokeData = GameInterop.GetVirtualFunctionByName(o.Ptr->source, "GetResult");
        var result = ((delegate* unmanaged<void*, short, Il2CppMethodInfo*, nint>)invokeData->methodPtr)(o.Ptr->source, GetToken(resultType), invokeData->method);
        return Marshal.PtrToStructure((nint)(&result), resultType)!;
    }

    private nint GetResultPtr()
    {
        if (o.Ptr->source == null) return (nint)((ECGen.Generated.Cysharp.Threading.Tasks.UniTask<nint>*)o.Ptr)->result;
        var invokeData = GameInterop.GetVirtualFunctionByName(o.Ptr->source, "GetResult");
        return ((delegate* unmanaged<void*, short, Il2CppMethodInfo*, nint>)invokeData->methodPtr)(o.Ptr->source, ((ECGen.Generated.Cysharp.Threading.Tasks.UniTask<nint>*)o.Ptr)->token, invokeData->method);
    }

    public void Dispose()
    {
        o.Dispose();
        GC.SuppressFinalize(this);
    }
}