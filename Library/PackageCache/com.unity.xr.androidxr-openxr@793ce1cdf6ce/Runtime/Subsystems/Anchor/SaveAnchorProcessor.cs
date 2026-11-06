using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.OpenXR.Features.Android
{
    internal class SaveAnchorProcessor : IRetryableTaskProcessor<Result<SerializableGuid>>
    {
        const int k_PersistentDataNotReady = -1000457003; //XR_ERROR_PERSISTED_DATA_NOT_READY_ANDROID

        public LinkedList<RetryableTaskData<Result<SerializableGuid>>> TasksData { get; } = new();

        void IRetryableTaskProcessor<Result<SerializableGuid>>.OnTimeOut(in RetryableTaskData<Result<SerializableGuid>> taskData)
        {
            taskData.CompletionSource.TrySetResult(new Result<SerializableGuid>(new XRResultStatus(XRResultStatus.StatusCode.UnknownError), taskData.Guid));
        }

        void IRetryableTaskProcessor<Result<SerializableGuid>>.TryNativeApiRequest(
            LinkedListNode<RetryableTaskData<Result<SerializableGuid>>> node,
            ref RetryableTaskData<Result<SerializableGuid>> taskData)
        {
            var status = new XRResultStatus();

            // Phase 1: Try to get a GUID, if it's not yet retrieved
            if (taskData.Guid == SerializableGuid.empty)
            {
                NativeApi.TrySaveAnchor(taskData.TrackableId, ref taskData.Guid, ref status);

                if (status.nativeStatusCode == k_PersistentDataNotReady)
                {
                    return;
                }

                if (status.IsError())
                {
                    Debug.LogWarning($"{GetType().Name}: Failed an attempt to initiate anchor persistence. StatusCode: {status.statusCode}, NativeStatusCode: {status.nativeStatusCode}");
                    node.List?.Remove(node);
                    taskData.CompletionSource.TrySetResult(new Result<SerializableGuid>(status, taskData.Guid));
                    return;
                }

                if (status.IsSuccess() && taskData.Guid != SerializableGuid.empty)
                {
                    // GUID assigned — stay in list for phase 2
                    node.Value = taskData;
                }
                return;
            }

            // Phase 2: Check if the anchor is persisted
            var saveCompletedResult = NativeApi.IsPersistedAnchor(taskData.Guid);

            if (saveCompletedResult.nativeStatusCode == k_PersistentDataNotReady)
            {
                return;
            }

            if (saveCompletedResult.IsSuccess())
            {
                var result = new Result<SerializableGuid>(new XRResultStatus(true), taskData.Guid);
                node.List?.Remove(node);
                taskData.CompletionSource.TrySetResult(result);
            }
        }
    }
}
