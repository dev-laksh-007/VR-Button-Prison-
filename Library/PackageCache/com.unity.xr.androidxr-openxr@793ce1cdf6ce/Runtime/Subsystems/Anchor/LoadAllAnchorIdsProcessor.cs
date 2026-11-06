using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.OpenXR.Features.Android
{
    internal class LoadAllAnchorIdsProcessor : IRetryableTaskProcessor<Result<NativeArray<SerializableGuid>>>
    {
        const int k_PersistentDataNotReady = -1000457003; //XR_ERROR_PERSISTED_DATA_NOT_READY_ANDROID

        public LinkedList<RetryableTaskData<Result<NativeArray<SerializableGuid>>>> TasksData { get; } = new();

        void IRetryableTaskProcessor<Result<NativeArray<SerializableGuid>>>.OnTimeOut(
            in RetryableTaskData<Result<NativeArray<SerializableGuid>>> taskData)
        {
            taskData.CompletionSource.TrySetResult(new Result<NativeArray<SerializableGuid>>(new XRResultStatus(XRResultStatus.StatusCode.UnknownError),
                new NativeArray<SerializableGuid>()));
        }

        void IRetryableTaskProcessor<Result<NativeArray<SerializableGuid>>>.TryNativeApiRequest(
            LinkedListNode<RetryableTaskData<Result<NativeArray<SerializableGuid>>>> node,
            ref RetryableTaskData<Result<NativeArray<SerializableGuid>>> taskData)
        {
            var countResultStatus = new XRResultStatus();
            var idCount = (int)NativeApi.GetSavedIdCount(ref countResultStatus);

            if (countResultStatus.nativeStatusCode == k_PersistentDataNotReady)
            {
                return;
            }

            if (countResultStatus.IsError() || idCount == 0)
            {
                if (countResultStatus.IsError())
                {
                    Debug.LogWarning($"{GetType().Name}: Failed to get saved IDs count. StatusCode: {countResultStatus.statusCode}, NativeStatusCode: {countResultStatus.nativeStatusCode}");
                }

                node.List?.Remove(node);
                taskData.CompletionSource.TrySetResult(new Result<NativeArray<SerializableGuid>>(countResultStatus, new NativeArray<SerializableGuid>(0, taskData.Allocator)));
                return;
            }

            var savedIds = new NativeArray<SerializableGuid>(idCount, taskData.Allocator);
            var savedIdsResultStatus = new XRResultStatus();

            unsafe
            {
                NativeApi.GetSavedIds(idCount, savedIds.GetUnsafePtr(), ref savedIdsResultStatus);
            }

            if (savedIdsResultStatus.nativeStatusCode == k_PersistentDataNotReady)
            {
                savedIds.Dispose();
                return;
            }

            if (savedIdsResultStatus.IsError())
            {
                savedIds.Dispose();
                Debug.LogWarning($"{GetType().Name}: Failed to get saved IDs. StatusCode: {savedIdsResultStatus.statusCode}, NativeStatusCode: {savedIdsResultStatus.nativeStatusCode}");
                node.List?.Remove(node);
                taskData.CompletionSource.TrySetResult(new Result<NativeArray<SerializableGuid>>(savedIdsResultStatus, new NativeArray<SerializableGuid>(0, taskData.Allocator)));
                return;
            }

            node.List?.Remove(node);
            taskData.CompletionSource.TrySetResult(new Result<NativeArray<SerializableGuid>>(savedIdsResultStatus, savedIds));
        }
    }
}
