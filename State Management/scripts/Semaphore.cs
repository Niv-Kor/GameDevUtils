using GameDevUtils.Math;
using System.Collections.Generic;
using UnityEngine.Events;

namespace GameDevUtils.ObjectManagement
{
    public class Semaphore
    {
        #region Class Members
        private ISet<int> requests;
        private UnityAction onFirstRequest;
        private UnityAction onEmptyStack;
        #endregion

        #region Properties
        public int RequestsCount => requests.Count;
        public bool IsEmpty => RequestsCount == 0;
        #endregion

        public Semaphore(UnityAction onFirstRequest = null, UnityAction onEmptyStack = null) {
            this.requests = new HashSet<int>();
            this.onFirstRequest = onFirstRequest;
            this.onEmptyStack = onEmptyStack;
        }

        /// <returns>A random layer ID. The ID can never be 0.</returns>
        private int GenerateHideRequestID() {
            int id;

            do id = ChanceUtils.Range(1, int.MaxValue);
            while (requests.Contains(id));

            return id;
        }

        /// <param name="requestId">The request ID to check</param>
        /// <returns>True if the given request ID is already active in the semaphore.</returns>
        public bool HasActiveRequest(int requestId) => requests.Contains(requestId);

        /// <summary>
        /// Add a request to this semaphore.
        /// </summary>
        /// <returns>An ID number that is needed to pull this request via <see cref="PullRequest(int)"/></returns>
        public int AddRequest() {
            int id = GenerateHideRequestID();
            requests.Add(id);

            if (requests.Count == 1) onFirstRequest?.Invoke();
            return id;
        }

        /// <summary>
        /// Cancel a previous request.
        /// </summary>
        /// <param name="requestId">An ID number that was generated in <see cref="AddRequest"/></param>
        /// <returns>True if the request has been successfully pulled.</returns>
        public bool PullRequest(int requestId) {
            if (!requests.Remove(requestId)) return false;

            if (requests.Count == 0) onEmptyStack?.Invoke();
            return true;
        }
    }
}