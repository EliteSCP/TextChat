# TextChat
An in-game TextChat plugin for SCP:SL.

## Minimum requirements
[EXILED](https://github.com/galaxy119/EXILED) **1.9.0+**

[LiteDB](https://github.com/mbdavid/LiteDB) **5.0.3+**

## How to install
Put **TextChat.dll** inside `%appdata%\Plugins` if you're on **Windows** or `~/.config/Plugins` on **Linux**.

Put **EXILED_Permissions.dll** inside `%appdata%\Plugins` if you're on **Windows** or `~/.config/Plugins` on **Linux**.

Put **LiteDB.dll** inside `%appdata%\Plugins\dependencies` folder if you're using **Windows** or `~/.config/Plugins/dependencies` if you're on **Linux**.

## How to use it in-game
Press **ò** or ` on your keyboard to open the in-game console, with that, you'll be able to execute commands to chat with other players.

### Player Commands
| Command | Description | Arguments | Example |
| --- | --- | --- | --- |
| .chat | Sends a chat message to everyone in the server. | **[Message]** | **.chat Hi all!** |
| .chat_team | Sends a chat message to your team. | **[Message]** | **.chat_team I love being SCP!** |
| .chat_private | Sends a private chat message to a player. | **[Nickname/UserID/PlayerID] [Message]** | **.chat_private iopietro Hello!** | 
| .help | Gets the list of commands or the description of a single command. | **[Command Name (optional)]** | **.help/.help chat_team** |


### Administrator Commands
| Command | Description | Arguments | Permission | Example |
| --- | --- | --- | --- | --- |
| chat_mute | Mute a player from the chat. | **[PlayerID/UserID/Name] [Duration (Minutes)] [Reason]** | tc.mute | **chat_mute iopietro 600 Spamming** |
| chat_unmute | Unmute a player from the chat. | **[PlayerID/UserID/Name]** | tc.unmute | **chat_unmute iopietro** |

### Configs
| Name | Type | Default Value | Description |
| --- | --- | --- | --- |
| tc_enabled | Boolean | True | Enable/Disable the plugin. |
| tc_database_name | String | TextChat | The name of the Database. |
| tc_general_chat_color | String | cyan | The color of the general chat. |
| tc_private_message_color | String | magenta | The color of private messages. |
| tc_max_message_length | Integer | 75 | The maximum length of a message. |
| tc_censor_bad_words | Boolean | False | If enabled, every message will be censored, by picking words from the bad words list. |
| tc_censor_bad_words_char | Char | * | The character used to censor messages. |
| tc_bad_words | String List | Empty | The list of words that will be censored in every message. |
| tc_save_chat_to_database | Boolean | True | If enabled, every message sent by players, will be saved into the database. |
| tc_can_spectator_send_messages_to_alive | Boolean | False | If enabled, spectators will be able to send messages to alive players. |
| tc_show_chat_muted_broadcast | Boolean | True | If enabled, a broadcast will alert the muted player. |
| tc_chat_muted_broadcast_duration | Unsigned Integer | 10 | The duration of the muted broadcast. |
| tc_chat_muted_broadcast | String | You have been muted from the chat for {0} minutes, reason: {1} | The broadcast message that  will be shown to the muted player (**{0}** and **{1}** are placeholders for the duration and the reasion of the mute). |
| tc_show_private_message_notification_broadcast | Boolean | False | If enabled, a broadcast is shown to players that receive private messages. |
| tc_private_message_notification_broadcast_duration | Unsigned Integer | 6 | The duration of the private message notification broadcast. |
| tc_private_message_notification_broadcast | String | You received a private message! | The broadcast message that will be shown to the notified player. |
| tc_is_slow_mode_enabled | Boolean | True | If enabled, a player will be able to send another message, only after a certain amount of time. |
| tc_slow_mode_interval | Float | 0.75 | The number of seconds that will have to pass before a player can send another message. |

### List of available colors
| Name |
| --- |
| yellow |
| green |
| cyan |
| red |
| magenta |
| black |
| white |
| blue |
| grey |

