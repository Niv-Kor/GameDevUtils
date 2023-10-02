using System.Collections.Generic;
using UnityEngine;

namespace GameDevUtils.EditorMode
{
    public class ChanneledLogger
    {
        #region Constants
        private static readonly LogChannel ALL_CHANNELS = (LogChannel)~0u;
        private static readonly Dictionary<LogChannel, string> CHANNEL_COLORS = new Dictionary<LogChannel, string> {
            { LogChannel.General, "black" },
            { LogChannel.AI, "green" },
            { LogChannel.Player, "red" },
            { LogChannel.UI, "orange" },
            { LogChannel.Audio, "blue" },
            { LogChannel.Network, "dark green" }
        };

        private static readonly Dictionary<LogPriority, string> PRIORITY_COLORS = new Dictionary<LogPriority, string> {
            { LogPriority.Info, "black" },
            { LogPriority.Warning, "orange" },
            { LogPriority.Error, "red" },
            { LogPriority.FatalError, "red" },
        };
        #endregion

        #region Class Members
        private static ChanneledLogger instance;
        private LogChannel activeChannels;
        #endregion

        #region Properties
        private static ChanneledLogger Instance => instance ?? (instance = new ChanneledLogger());
        #endregion

        private ChanneledLogger() {
            this.activeChannels = ALL_CHANNELS;
        }

        /// <summary>
        /// Reset all log channels.
        /// </summary>
        public static void ResetChannels() {
            Instance.activeChannels = ALL_CHANNELS;
        }

        /// <summary>
        /// Clear all log channels.
        /// </summary>
        public static void ClearChannels() {
            Instance.activeChannels = 0;
        }

        /// <param name="channel">The channel to enable</param>
        public static void EnableChannel(LogChannel channel) {
            Instance.activeChannels |= channel;
        }

        /// <param name="channel">The channel to disable</param>
        public static void DisableChannel(LogChannel channel) {
            Instance.activeChannels &= ~channel;
        }

        /// <param name="channel">The channel to toggle</param>
        public static void ToggleChannel(LogChannel channel) {
            Instance.activeChannels ^= channel;
        }

        /// <param name="channel">The channel to check</param>
        /// <returns>True if the given channel is enabled.</returns>
        public static bool IsChannelActive(LogChannel channel) {
            return (Instance.activeChannels & channel) == channel;
        }

        /// <param name="channels">The channels to set (flag enum value)</param>
        public static void SetChannels(LogChannel channels) {
            Instance.activeChannels = channels;
        }

        /// <returns>The currently active channels.</returns>
        public static LogChannel GetActiveChannels() => Instance.activeChannels;

        /// <see cref="Log(LogChannel, LogPriority, string)" />
        public static void Log(string message) {
            Log(LogChannel.General, LogPriority.Info, message);
        }

        /// <see cref="Log(LogChannel, LogPriority, string)" />
        public static void Log(LogChannel logChannel, string message) {
            Log(logChannel, LogPriority.Info, message);
        }

        /// <summary>
        /// Log a message.
        /// </summary>
        /// <param name="logChannel">The log's channel</param>
        /// <param name="priority">The log's priority level</param>
        /// <param name="message">The message to log</param>
        public static void Log(LogChannel logChannel, LogPriority priority, string message) {
            if (!IsChannelActive(logChannel)) return;

            string finalMessage = ContructLogString(logChannel, priority, message, priority != LogPriority.FatalError);

            switch (priority) {
                case LogPriority.FatalError:
                case LogPriority.Error:
                    Debug.LogError(finalMessage);
                    break;

                case LogPriority.Warning:
                    Debug.LogWarning(finalMessage);
                    break;

                case LogPriority.Info:
                    Debug.Log(finalMessage);
                    break;
            }
        }

        /// <param name="logChannel">The log's channel</param>
        /// <param name="priority">The log's priority level</param>
        /// <param name="message">The message to log</param>
        /// <param name="colorize">True to colorize the log message</param>
        /// <returns>A formatted log string.</returns>
        private static string ContructLogString(LogChannel logChannel, LogPriority priority, string message, bool colorize = true) {
            string priortiyColor = PRIORITY_COLORS[priority];

            if (!CHANNEL_COLORS.TryGetValue(logChannel, out string channelColor)) {
                channelColor = "black";
                Debug.LogErrorFormat("Please define a color for channel {0}.", logChannel);
            }

            if (colorize) {
                string rawFormat = "<b><color={0}>[{1}] </color></b> <color={2}>{3}</color>";
                return string.Format(rawFormat, channelColor, logChannel, priortiyColor, message);
            }
            else return string.Format("[{0}] {1}", logChannel, message);
        }
    }
}