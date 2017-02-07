/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Dorian Stoll 2017
 * Licensed under the terms of the MIT License
 */

using System;

// This is german. I dont care
namespace HVH.Service.Connection
{
    /// <summary>
    /// The communication protocol for Client and Server
    /// </summary>
    public static class Communication
    {
        //------------------------new--------------------------
        public const String Empty = "Empty";
        //------------------------new--------------------------

        /// <summary>
        /// IdentifikationsID des Programms
        /// </summary>
        public const String CLIENT_ID                = "9033fa9938484bf9ae1b27b4ff9ed8de";
        /// <summary>
        /// Client sendet als nächstes den Public-Key
        /// </summary>
        public const String CLIENT_SEND_PUBLIC_KEY   = "198edb63031e422fbde1760bb13bd87a";
        /// <summary>
        /// Client sendet als nächstes seine Eigenschaften
        /// </summary>
        public const String CLIENT_SEND_SESSION_DATA = "68d986b0ec164908ab060aae242aefac";
        /// <summary>
        /// Client sendet auf Anfrage des servers ein Signal, das er noch da ist.
        /// </summary>
        public const String CLIENT_SEND_HEARTBEAT    = "65e52334b16b4f70a1066001a708769f";

        /// <summary>
        /// IdentifikationsID des Serverprogramms
        /// </summary>
        public const String SERVER_ID                              = "1079f3e4be3647dcae9302e73b7604d5";
        /// <summary>
        /// Der server sendet als nächstes den SessionKey
        /// </summary>
        public const String SERVER_SEND_SESSION_KEY                = "3b635b374e3f4060bdaba35090a117a8";
        /// <summary>
        /// Server sendet das die Session erfolgreich erstellt wurde
        /// </summary>
        public const String SERVER_SEND_SESSION_CREATED            = "7289d8a91dcb456b871c428d164f53ba";
        /// <summary>
        /// Server fordert LifeSign vom Client
        /// </summary>
        public const String SERVER_SEND_HEARTBEAT_CHALLENGE        = "e14d04eca02d4c8cb8d3c79c5b720158";
        /// <summary>
        /// Server teilt dem client mit, dass er sich herunterfahren soll
        /// </summary>
        public const String SERVER_SEND_SHUTDOWN                   = "c6d1f3ff26bb4038b2c7d18456fbd6f1";
        /// <summary>
        /// server teilt dem client mit, dass er sich neustarten soll
        /// </summary>
        public const String SERVER_SEND_REBOOT                     = "52fe0e9ff2774712b6ea3b76e5775aef";
        /// <summary>
        /// Server sendet die Aufforderung die gesendeten Prozesse an die bestehende Liste der verbotenen Prozesse anzufügen
        /// </summary>
        public const String SERVER_SEND_FORBIDDEN_PROCESSES        = "7bd5e72b747a463986443378dd725fe4";
        /// <summary>
        /// Server sendet Aufforderung die Liste der verbotetenen Prozesse zu löschen und eine neue zu beginnen
        /// </summary>
        public const String SERVER_SEND_FORBIDDEN_PROCESSES_CLEAR  = "5fc8feb816d440fda22a18b73e36b22a";
        /// <summary>
        /// Server sendet die Nachricht, dass ein update verfügbar ist
        /// </summary>
        public const String SERVER_SEND_UPDATE_MESSAGE             = "3c8c5b376088425588bc5a58cdfa477a";
        /// <summary>
        /// Server sendet die Aussage an den Client das dieser auf das nächste Paket warten soll
        /// </summary>
        public const String SERVER_SEND_WAIT_SIGNAL                = "f9b257264e2e4cd4be4f16fc3d06037b";
        /// <summary>
        /// Server sendet das Signal zum disconnecten
        /// </summary>
        public const String SERVER_SEND_DISCONNECTION              = "b503a21db88c40a0b2195cbdc913651b";
        /// <summary>
        /// Server sendet das Signal, dass der PC gesperrt werden soll
        /// </summary>
        public const String SERVER_SEND_LOCK                       = "d9bffdc37f634621932619165a661686";
        /// <summary>
        /// Server sendet das Signal, dass der PC entsperrt werden soll
        /// </summary>
        public const String SERVER_SEND_UNLOCK                     = "b369a2aa5bbc46a59bd1bcde6bb6bb63";
        /// <summary>
        /// Server sendet dass die Login Daten ungültig sind
        /// </summary>
        public const String SERVER_SEND_LOGIN_INVALID              = "10ff071cbadc425d81f2b6ce662963f0";
        /// <summary>
        /// Server sendet dass die Login Daten gültig sind und der Benutzer angemeldet wurde
        /// </summary>
        public const String SERVER_SEND_LOGIN_SUCCESS              = "b2d28458647f49bc8e5406d8a3d24e46";
        /// <summary>
        /// Server sendet dass momentan keine Anmeldung möglich ist, da keine Anmeldeserver zur Verfügung stehen
        /// </summary>
        public const String SERVER_SEND_NO_LOGIN_SERVERS           = "b9520c196fed4062aed9fb07bcbb848b";
        /// <summary>
        /// Server sendet, dass das Öfnnen der Datenbank gestarted wurde
        /// </summary>
        public const String SERVER_SEND_ACCESSING_DATABASE         = "8a42970f677a4ca3b96180d35ffd5413";
        /// <summary>
        /// Server sendet dass der Benutzer ausgelogged ist
        /// </summary>
        public const String SERVER_SEND_LOGGEDOUT                  = "d03adb9affe044668bc3beb790538440";
        /// <summary>
        /// Server sendet sie Liste der in der Datei vorhandenen Räume
        /// </summary>
        public const String SERVER_SEND_ROOMS                      = "d4bffbcede9a4db3b6bdba08cca15a5a";
        /// <summary>
        /// Server sendet dass nun alle Räume übertragen wurden
        /// </summary>
        public const String SERVER_SEND_ROOMS_FINISHED             = "d2164ebe9f9d4bd2b1044221f939d928";
        /// <summary>
        /// Server sendet die Anfrage an den Adminclient nach dem Passwort für die Datenbank
        /// </summary>
        public const String SERVER_SEND_DATABASE_PASSCODE_REQUEST  = "822654023c4f4608a7a1588956d94906";
        /// <summary>
        /// Server sendet als nächstes die Liste der gestarteten Computer
        /// </summary>
        public const String SERVER_SEND_STARTED_CLIENTS            = "cab3678e90ba4083884442eb264742dc";
        /// <summary>
        /// Sever sendet, dass nun alle gestarteten Clients übertragen wurden
        /// </summary>
        public const String SERVER_SEND_STARTED_CLIENTS_FINISHED   = "a613723ddad64acab8b341cf136660c4";
        /// <summary>
        /// Server sendet die Liste der gesperrten Clients
        /// </summary>
        public const String SERVER_SEND_LOCKED_CLIENTS             = "25e6c186498c4a95a4735dc61a23e3e0";
        /// <summary>
        /// Server sendet, dass nun die Namen aller gesperrten Clients übertragen wurden
        /// </summary>
        public const String SERVER_SEND_LOCKED_CLIENTS_FINISHED    = "ae85e61ef47046008316294901c37e17";
        /// <summary>
        /// Server sendet ein Fehler Singal an den Client, dass de gefordete Aufgabe momentan nicht abgeschlossen werden kann
        /// </summary>
        public const String SERVER_SEND_FAILURE_SIGNAL             = "65b37071e4334ea2b8daeee1a20589ab";

        // ---------------------- new --------------------- //
        /// <summary>
        /// Server sendet dass der Benutzeraccount disabled ist
        /// </summary>
        public const String SERVER_SEND_DISABLED_USERACCOUNT = "2e392685b4ad47958e832cd9746d3ed6";
        /// <summary>
        /// Server beginnt mit dem Senden der MainFolders
        /// </summary>
        public const String SERVER_SEND_MAINFOLDERS = "12cbf987a2c54ed5814984c54e319d8a";
        /// <summary>
        /// Server hat alle MainFolders gesendet
        /// </summary>
        public const String SERVER_SEND_MAINFOLDERS_FINISHED = "53e24e6f8d484f80b2b8891571abe6bf";
        /// <summary>
        /// Server beginnt die FolderIDs zu senden
        /// </summary>
        public const String SERVER_SEND_FOLDER_IDS = "51489aac2f4c4c2c9298e0d08cef1ac0";
        /// <summary>
        /// Server hat alle FolderIDs erfolgreich gesendet
        /// </summary>
        public const String SERVER_SEND_FOLDER_IDS_FINISHED = "dc2017a49b62495e85c276c02a0925a7";
        /// <summary>
        /// Server beginnt mit dem senden der Ordnernamen
        /// </summary>
        public const String SERVER_SEND_FOLDER_NAMES = "2aa1c93f44114bad872b183305225e17";
        /// <summary>
        /// Server hat alle Ordnernamen erfolgreich gesendet
        /// </summary>
        public const String SERVER_SEND_FOLDER_NAMES_FINISHED = "c9b55eabba5c4e3f8c3964d885993fc6";
        /// <summary>
        /// Server beginnt mit dem senden der Dateinamen
        /// </summary>
        public const String SERVER_SEND_FILE_NAMES = "ef127ee9fa2747839c7a452ff6d84ea6";
        /// <summary>
        /// Server hat alle dateinamen gesendet
        /// </summary>
        public const String SERVER_SEND_FILE_NAMES_FINISHED = "1ec7c7715a3f4f81a729da10945df114";
        /// <summary>
        /// Server beginnt mit dem senden Der FileIDs
        /// </summary>
        public const String SERVER_SEND_FILE_IDS = "be0026e205444919a697fb8af56e41ef";
        /// <summary>
        /// Server hat alle FileIDs gesendet
        /// </summary>
        public const String SERVER_SEND_FILE_IDS_FINISHED = "3f1428987790424cb9ffe813c629f276";
        /// <summary>
        /// Server beginnt mit dem senden des Ordnerpfades
        /// </summary>
        public const String SERVER_SEND_FOLDERPATH = "c24ad2c4ed4c412a85c4034c08007105";
        /// <summary>
        /// Server hat das senden des ordnerpfades abgeschlossen
        /// </summary>
        public const String SERVER_SEND_FOLDERPATH_FINISHED = "715ec8fc4c804e168caf8ee1b0da90f6";
        public const String SERVER_SEND_NEW_FOLDER_CREATED = "d2b49c496aa749238e8ef3d2465f8022";
        public const String SERVER_SEND_FOLDER_MOTHERDIR_INFO = "ae5125cc41b447269ce70368f7013a32";
        public const String SERVER_SEND_GROUP_NAMES = "d62d1ed8e57543bba9d554af72e55f13";
        public const String SERVER_SEND_GROUP_NAMES_FINISHED = "505fd15905934910b3670abcd2ebd703";
        public const String SERVER_SEND_GROUP_IDS = "a0f24735026342889361b6c782af2867";
        public const String SERVER_SEND_GROUP_IDS_FINISHED = "6586b9459df24f2a8ccb4f3ccc1ff29f";
        public const String SERVER_SEND_USER_NAMES = "bc3233b7e5ae4c319c7624aaaa8a88b4";
        public const String SERVER_SEND_USER_NAMES_FINISHED = "acd72978617949c599085daa428dc78e";
        public const String SERVER_SEND_USER_IDS = "94e41cb29cc145039d6a91c10c337bf2";
        public const String SERVER_SEND_USER_IDS_FINISHED = "e4e8106ad0234931ae5806e41f5fe5f0";
        public const String SERVER_SEND_USER_REALNAMES = "e10b316f6efa4ec7a10cab14fec4ba8b";
        public const String SERVER_SEND_USER_REALNAMES_FINISHED = "65834d7b616346bb872dfc4c45be0aac";
        public const String SERVER_SEND_IS_DELETING = "03ab973d31264e089f1654bb4856d3ed";
        public const String SERVER_SEND_GETBYTES_START = "719bd26454944001aebf530ae9fa062d";
        public const String SERVER_SEND_RECIEVED_BYTES = "61b764e732ad403793e2d353240db500";
        public const String SERVER_SEND_DIDNT_RECIEVED_BYTES = "8de50613a9e641f081acf0c776ebad92";
        public const String SERVER_SEND_NOT_ENOUGH_SPACE = "0a97abbb3f1e479e96d43e7f855010de";
        public const String SERVER_SEND_STARTED_ADDING = "9d609691acbe49b8a7737843518e624d";
        public const String SERVER_SEND_STARTED_DEFRAGMENTING = "89c779acc21140f1b01e7446b64189f6";
        public const String SERVER_SEND_CANNOT_START_DEFRAGMENTION = "fa2cddbacb70402a88d4028291d9595f";
        public const String SERVER_SEND_DEFRAGMENTING_IS_RUNNING = "b7343937af8f43409a5b08fcefc11bef";
        public const String SERVER_SEND_DONT_DEFRAGMENTING = "a7fc1c941fb74e15bcae11e759dba128";
        public const String SERVER_SEND_FILESIZES = "";
        public const String SERVER_SEND_FILESIZES_FINISHED = "";
        public const String SERVER_SEND_FILEBYTES = "";
        public const String SERVER_SEND_FILEBYTES_FINIISHED = "";
        public const String SERVER_SEND_FILEPATHES = "";
        public const String SERVER_SEND_FILEPATHES_FINISHED = "";
        public const String SERVER_SEND_ADDING_TIMES = "";
        public const String SERVER_SEND_ADDING_TIMES_FINISHED = "";
        public const String SERVER_SEND_CREATION_TIMES = "";
        public const String SERVER_SEND_CREATION_TIMES_FINISHED = "";
        public const String SERVER_STARTS_SENDING_FILEBYTES = "";
        public const String SERVER_FINISHED_SENDING_FILEBYTES = "";
        public const String SERVER_SEND_MAXSIZE = "";
        public const String DATA_SEND_OK = "";
        public const String DATA_SEND_GET_SENDING_FILEBYTES_STARTED = "";
        public const String DATA_SEND_DOWNLOAD_FOLDER_REQUEST = "";
        public const String DATA_SEND_DOWNLOAD_FILE_REQUEST = "";
        public const String DATA_SEND_DEFRAGMENT_STATUS_REQUEST = "6dca24bed00945c7b000d7225a151a15";
        public const String DATA_SEND_DEFRAGMENT = "b13ae2f751b24d94b61ee7c1d6007128";
        public const String DATA_SEND_ABORT_SIGNAL = "d422b23714e54448b48c955aac9c5f99";
        public const String DATA_SEND_BYTES = "88319bbdc7354f52b4f9ab63241ffd9b";
        public const String DATA_SEND_BYTES_FINISHED = "ee4205018e6e4b8a9846c31b7c81f546";
        public const String DATA_SEND_ADDFILE_REQUEST = "a8800f6c9f5646d7b86e989cac4bc4fc";
        public const String DATA_SEND_CREATE_NEW_USER = "6b2c5e0f0e594e148c95898261e5a396";
        public const String DATA_SEND_CREATE_NEW_FOLDER_NEW_FOLDER_METHOD = "ffa75c9559834d6bbc850f9ba4a5b42e";
        public const String DATA_SEND_SEARCH_USERS_REQUEST = "661f72858dde424783a6a8335bf98b0e";
        public const String DATA_SEND_USER_DELETE_REQUEST = "cdc2baf991514b249fc2b378fe439575";
        public const String DATA_SEND_USERS_REQUEST = "cc946d0a194e4e089da4052489771880";
        public const String DATA_SEND_GROUPS_REQUEST = "01994702081946b787d6454c73a9edb7";
        public const String DATA_SEND_SEARCH_REQUEST = "c91c96b8e2934fe0930e8d0267e2d2d7";
        public const String DATA_SEND_FOLDER_MOTHERDIR_INFO_REQUEST = "9503f367545340eb91821a185ec20bc8";
        public const String DATA_SEND_GROUPIDS = "11e1c7200c834ea182456a84e8b73c00";
        public const String DATA_SEND_GROUPIDS_FINISHED = "8e6abcdf5e61444587d3fce00c1b80d7";
        public const String DATA_SEND_CREATE_NEW_FOLDER = "9781f75bdb244f218a9b3e8214fe1343";
        public const String DATA_SEND_FILE_OF_FOLDER_REQUEST = "d2306f4f04e043a68bce1f057828311f";
        public const String DATA_SEND_SUBDIRECTORIES_REQUEST = "0beac64eaa9e454eb3143b0f1402295e";
        public const String DATA_SEND_FOLDERPATH_REQUEST = "52c4ab9ae831488489a5def27f2b367d";
        public const String DATA_ID = "f6f30dfd2aa845be93a7431c14a18d79";
        public const String DATA_SEND_PUBLIC_KEY = "2498a837755440cda0dd0f71ea620b03";
        public const String DATA_SEND_USERDATA = "80a71410314648299b743e3f74f019fb";
        public const String DATA_SEND_LOGIN_REQUEST = "03334ff39ac441f9963f1241ea8d8156";
        public const String DATA_SEND_LOGOUT_REQUEST = "395ba5b50ce4401ea3a9bc37d5168e90";
        public const String DATA_SEND_SESSION_KEY = "5d96c9dd6482412a8d82614af07374a1";
        public const String DATA_SEND_HEARTBEATCHALLENGE = "c2708ef19df24df8ae81e0a31d71252b";
        public const String DATA_SEND_DISCONNECT_SIGNAL = "9f18aef7fa6948359696283e9a954faa";
        public const String DATA_SEND_MAINFOLDERS_REQUEST = "7db5035149ca4623bfe0cea8f5035adf";

        // ---------------------- new --------------------- //

        /// <summary>
        /// IdentifikationsID des Adminprogramms
        /// </summary>
        public const String ADMIN_ID                               = "2a2b2c21f37c48d48e3bf41583d74d8f";
        /// <summary>
        /// AdminClient fragt den Server nach den vorhandenen Räumen
        /// </summary>
        public const String ADMIN_SEND_ROOMS_REQUEST               = "cc95c12cf2c940a688ee3d7b57dee084";
        /// <summary>
        /// Admin sendet seinen PublicKey
        /// </summary>
        public const String ADMIN_SEND_PUBLIC_KEY                  = "d858ed23b1f7426891c5471208e73550";
        /// <summary>
        /// AdminClient sendet als nächstes seine SystemInformationen
        /// </summary>
        public const String ADMIN_SEND_USERDATA                    = "0fbd883b9fbc4b95b6f3f217e6a85d64";
        /// <summary>
        /// AdminClient möchte sich auf den Server einloggen
        /// </summary>
        public const String ADMIN_SEND_LOGIN_REQUEST               = "eb8086805a26417e9b6d0217ee3c6aa5";
        /// <summary>
        /// AdminClient möchte sich vom Server ausloggen
        /// </summary>
        public const String ADMIN_SEND_LOGOUT_REQUEST              = "f4546b97b0284368acbfc3d1156ca3a0";
        /// <summary>
        /// AdminClient möchte die Liste der gestarteten Computer haben
        /// </summary>
        public const String ADMIN_SEND_STARTED_CLIENTS_REQUEST     = "59fcfe339b8342498ea11243d4ae479d";
        /// <summary>
        /// AdminClient möchte die Liste der gesperrten Computer haben
        /// </summary>
        public const String ADMIN_SEND_LOCKED_CLIENTS_REQUEST      = "88d10353d5234c34a9a35172fc0e66ef";
        /// <summary>
        /// AdminClient sendet einen neuen Sessionkey
        /// </summary>
        public const String ADMIN_SEND_SESSION_KEY                 = "2a509ef8b7504a4d967c93da16b06215";
        /// <summary>
        /// Der Admin beginnt den HeartBeatChallenge Prozess
        /// </summary>
        public const String ADMIN_SEND_HEARTBEATCHALLENGE          = "089b11b6331945afa99bb689e7d5d9a3";
        /// <summary>
        /// AdminClient sendet dem Server das Signal, dass es sich jetzt disconnecten wird
        /// </summary>
        public const String ADMIN_SEND_DISCONNECT_SIGNAL           = "025d6c36604649f0a7c95e93417f710b";
        /// <summary>
        /// Adminclient sendet die Liste 
        /// </summary>
        public const String ADMIN_SEND_LOCK_COMPUTERS              = "fbe9d76ea21240008e53564c46fcab73";
        /// <summary>
        /// AdminCLient hat alle für die Sperrung vorgesehenen Rechner gesendet
        /// </summary>
        public const String ADMIN_SEND_LOCK_COMPUTERS_FINISHED     = "fde280aaae754650b39f2e5f8ba70d0d";
        /// <summary>
        /// Adminclient sendet die Liste 
        /// </summary>
        public const String ADMIN_SEND_UNLOCK_COMPUTERS            = "55455e374cee41979d284f73dde22904";
        /// <summary>
        /// AdminCLient hat alle für die Entsperrung vorgesehenen Rechner gesendet
        /// </summary>
        public const String ADMIN_SEND_UNLOCK_COMPUTERS_FINISHED   = "d28feb7530b941eeafbd97d5d2361e8b";
        /// <summary>
        /// AdminClient sendet die Liste der herunterzufahrenden Clients
        /// </summary>
        public const String ADMIN_SEND_SHUTDOWN_COMPUTERS          = "6158ee8f31ff4dfd8064647e3a69b030";
        /// <summary>
        /// AdminClient hat die heruterfahrliste komplett gesendet
        /// </summary>
        public const String ADMIN_SEND_SHUTDOWN_COMPUTERS_FINISHED = "d79a5397442b4993a3db71f4b26284f0";
        /// <summary>
        /// AdminClient sendet die Liste der neuzustartenden PCs
        /// </summary>
        public const String ADMIN_SEND_RESTART_COMPUTERS           = "777b68591d0e4ac3afcf5018c59bfe22";
        /// <summary>
        /// AdminClient hat die Liste der neuzustartenden PCs vollständig übertragen
        /// </summary>
        public const String ADMIN_SEND_RESTART_COMPUTERS_FINISHED  = "865a1dd6b2394a6cb5017093f06b77bc";
        /// <summary>
        /// Der Admin weist den Server an jeden Client der gerade Connected ist automatisch herunterzufahren und jeden Client der sich neu connected auch
        /// </summary>
        public const String ADMIN_SEND_GENERAL_SHUTDOWN_COMPUTERS  = "b0a8ae431ddb44aca6af675e0607addd";
        /// <summary>
        /// Der AdminClient weist den Server ab sofort einen Client der sich connected nicht mehr sofort herunterzufahren
        /// </summary>
        public const String ADMIN_SEND_GENERAL_SHUTDOWN_COMPUTERS_ABORT  = "58d6d58530414b9994417f3006001dc0";
        /// <summary>
        /// Der AdminClient weist den Server an alle connecteten Clients neuzustarten
        /// </summary>
        public const String ADMIN_SEND_RESTART_ALL                       = "7f8b8ae18fa944e7beba0ce93d31aac8";
        /// <summary>
        /// Ser Adminclient weist den Server an alle connecteten Clients auszuschalten
        /// </summary>
        public const String ADMIN_SEND_SHUTDOWN_ALL                      = "e77523131aa44bac9504f13048f3575e";
        /// <summary>
        /// Der AdminClient sendet die neuen Daten eines Computers
        /// </summary>
        public const String ADMIN_SEND_COMPUTER_UPDATE                   = "6ba3ccb747ca4b4f809cf463f34d7f67";

        /**
         * -> Client Connects to Server
         * -> CLIENT_SEND_PUBLIC_KEY -- Public Key
         * -> SERVER_SEND_SESSION_KEY -- Session Key
         * -> CLIENT_SEND_SESSION_DATA -- PC Name and Username + Client ID
         * -> SERVER_SEND_SESSION_CREATED -- Server ID
         * -> ...
         * In a loop:
         *      -> SERVER_SEND_HEARTBEAT_CHALLENGE -- Random value
         *      -> CLIENT_SEND_HEARTBEAT -- Same random value
         */
    }
}
