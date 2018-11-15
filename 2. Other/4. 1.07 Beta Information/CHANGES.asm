;**************************************************
; D2CLIENT.DLL
;**************************************************
; Fix invalid packet IDs: subtract one from packets 0x60-0x6F
1000C47E	C64424 0C 65		MOV BYTE PTR SS:[LOCAL.11],65		; 64
1000C59A	C64424 0F 69		MOV BYTE PTR SS:[LOCAL.0+3],69		; 68
1000C5E3	C64424 04 6B		MOV BYTE PTR SS:[LOCAL.2],6B		; 6A
1000C63A	C64424 0F 67		MOV BYTE PTR SS:[LOCAL.0+3],67		; 66
1000C6C3	C64424 00 66		MOV BYTE PTR SS:[LOCAL.7],66		; 65
1000C8C5	C64424 28 6A		MOV BYTE PTR SS:[ESP+28],6A		; 69
1000C98C	C64424 28 6A		MOV BYTE PTR SS:[ESP+28],6A		; 69
10037857	B1 62			MOV CL,62				; 61
10042BC1	B1 61			MOV CL,61				; 60
10043C23	B1 64			MOV CL,64				; 63
100486C8	B1 61			MOV CL,61				; 60
1004A5A3	B1 61			MOV CL,61				; 60
1004B7AC	B1 62			MOV CL,62				; 61
1004B857	B1 62			MOV CL,62				; 61
10056491	B1 63			MOV CL,63				; 62

; Fix game dropping after returning from the escape menu: NOP code
; *The game sends an invalid packet (0x60) which tricks the server into thinking the player
;  used a hack, thereby disconnecting them
1008C9DD	3BC5			CMP EAX,EBP				; JMP SHORT 1008CA49
1008C9DF	74 05			JE SHORT 1008C9E6			; \ NOP
1008CA44	E8 071C0300		CALL 100BE650				; / (...)

; Fix game dropping when moving a belt item that has an item on top: NOP code
; *This could be an actual bug from leftover Classic code
1009D345	6A 00			PUSH 0					; JMP SHORT 1009D35A
1009D347	55			PUSH EBP				; \ NOP
1009D355	E8 360D0200		CALL 100BE090				; / (...)

; [EXTRA]
; Greatly reduce CPU usage in-game: force sleep
100023E3	75 08			JNZ SHORT 100023ED			; NOP
100023E5	6A 00			PUSH 0					; 0A

; Greatly reduce loading time for TCP/IP games: lower sleep time
1000C94D	68 FA000000		PUSH 0FA				; 40

;**************************************************
; D2COMMON.DLL
;**************************************************
; Suppress crashing caused by two bugs within the beta: force JMP
; *D2EXP.MPQ (MagicSuffix.txt) -> "of Evisceration" exceeds 63 max damage on 1h+2h/2h weapons
; *D2EXP.MPQ (Weapons.txt) -> Spawning of always-unique Phase Blades
6FDE48B8	72 1E			JB SHORT 6FDE48D8			; JMP SHORT 6FDE48D8
6FDE48DA	7D 1E			JGE SHORT 6FDE48FA			; JMP SHORT 6FDE48FA

;**************************************************
; D2GAME.DLL <PATCH 1.07>
;**************************************************
; Fix game version when joining a multiplayer game: 0x07 (1.07) -> 0x29 (1.41)
6FB61917	83FD 07			CMP EBP,7				; 29

; Fix game save/item version: 0x57 (1.07) -> 0x54 (1.41)
6FB7E558	6A 57			PUSH 57					; 54
6FB7E6A8	6A 57			PUSH 57					; 54
6FBAB4DA	C74424 24 57000000	MOV DWORD PTR SS:[ESP+24],57		; 54
6FBB0CBF	6A 57			PUSH 57					; 54
6FBDDA3E	6A 57			PUSH 57					; 54
6FBDDA87	6A 57			PUSH 57					; 54

; Fix save decoding/conversion: cases 0x47-0x57 (1.00-1.07) -> 0x47-0x54 (1.00-1.41)
6FBAC508	83F8 10			CMP EAX,10				; 0D
6FBAD2D2	83FF 10			CMP EDI,10				; 0D
6FBAD370	83F8 10			CMP EAX,10				; 0D
6FBAD484	83FF 10			CMP EDI,10				; 0D
6FBAD97E	83FE 10			CMP ESI,10				; 0D

; Fix call: D2Common.#11072 (Related to unit spawning)
6FB8BA24	8A46 4D			MOV AL,BYTE PTR DS:[ESI+4D]		; NOP
6FB8BA2B	50			PUSH EAX				; NOP

; Fix call: D2Common.#10525 (Inserting something into a socketed item)
6FB77DB2	56			PUSH ESI				; NOP

; Fix incorrect string numbers in string.tbl: anything past 0x0510, subtract 0x0151
6FB86F3A	68 7C0D0000		PUSH 0D7C				; 0C2B : Hireling overhead "I feel much stronger now"
6FB983D0	81C2 630E0000		ADD EDX,0E63				; 0D12 : Character overhead "%s" shrine fortune

; [EXTRA]
; Re-base initial loading address to optimize memory: 0x6FC60000 -> 0x6FB60000
; *The memory structure is different in 1.07. The base memory address was changed to allow
;  D2GAME.DLL to load at its intended address for 1.41.

;**************************************************
; D2GFX.DLL
;**************************************************
; [EXTRA]
; Allow multiple windows: force JMP
6FB3441C	74 47			JZ SHORT 6FB34465			; JMP SHORT 6FB34465

;**************************************************
; D2LAUNCH.DLL
;**************************************************
; Fix invisible overlay on title screen buttons to enable all options: NOP code
6FB07C6A	33D2			XOR EDX,EDX				; NOP
6FB07C79	E8 50CA0000		CALL <JMP.&D2Win.#10025>		; JMP SHORT 6FB07C90, NOP
6FB07C7E	33D2			XOR EDX,EDX				; \ NOP
6FB07C8B	E8 3ECA0000		CALL <JMP.&D2Win.#10025>		; / (...)

; Allow Hardcore

; Show Hardcore Button Checkbox (Offset: A4C4)
6FB0A4C4 | 75 1A                    | JNE d2launch.6FB0A4E0                   | ; JMP

; Enable Hardcore Checkbox and Text (Offset: AEA4)
6FB0AEA4 | 74 20                    | JE d2launch.6FB0AEC6                    | ; NOP

;**************************************************
; D2LAUNCH.DLL
;**************************************************
; Prevent Connect to Battle.Net
03AA1235 | 83 EC 18                 | SUB ESP,18                              | ; XOR EAX, EAX, and RET the NOP.

;**************************************************
; D2NET.DLL
;**************************************************
; Fix invalid packet IDs: subtract one from packets 0x60-0x6F
6FC81E78	80FA 6F			CMP DL,6F				; 6E
6FC81EBE	80FA 6A			CMP DL,6A				; 69
6FC81FC2	80FB 6F			CMP BL,6F				; 6E
6FC81FFD	80FB 65			CMP BL,65				; 64
6FC8201A	80FB 6F			CMP BL,6F				; 6E
6FC820B0	C64424 13 6E		MOV BYTE PTR SS:[LOCAL.0+3],6E		; 6D

; Fix packet size for packets 0x60-0x6D: move NULL packet to the end
6FC87564	05000000		DD 5					; 01000000 : xx -> 60 [D2CLTSYS_SWITCHWEAPONS]
6FC87568	01000000		DD 1					; 03000000 : 60 -> 61 [D2CLTSYS_MERCITEM]
6FC8756C	03000000		DD 3					; 05000000 : 61 -> 62 [D2CLTSYS_MERCREVIVE]
6FC87570	05000000		DD 5					; 05000000 : 62 -> 63 [D2CLTSYS_SHIFTBELTITEM]
6FC87574	05000000		DD 5					; 2E000000 : 63 -> 64 [D2CLTSYS_NEWGAME]
6FC87578	2E000000		DD 46					; 1D000000 : 64 -> 65 [D2CLTSYS_JOINGAME]
6FC8757C	1D000000		DD 29					; 01000000 : 65 -> 66 [D2CLTSYS_ENDGAME]
6FC87580	01000000		DD 1					; 01000000 : 66 -> 67 [D2CLTSYS_JOINLIST]
6FC87584	01000000		DD 1					; 01000000 : 67 -> 68 [D2CLTSYS_JOINACT]
6FC87588	01000000		DD 1					; FFFFFFFF : 68 -> 69 [D2CLTSYS_OPENCHAR]
6FC8758C	FFFFFFFF		DD -1					; 09000000 : 69 -> 6A [D2CLTSYS_CREATEGAME]
6FC87590	09000000		DD 9					; 01000000 : 6A -> 6B [D2CLTSYS_FORCEDISCONNECT]
6FC87594	01000000		DD 1					; 00000000 : 6B -> 6C [?]
6FC87598	00000000		DD 0					; 01000000 : 6C -> 6D [D2CLTSYS_DISCONNECT]
6FC8759C	01000000		DD 1					; 00000000 : 6D -> xx [NULL]

;**************************************************
; D2WIN.DLL
;**************************************************
; [EXTRA]
; Greatly reduce CPU usage in main menu: add sleep code
6F9AE50D	E8 4E060000		CALL 6F9AEB60				; CALL 6F9BADD0

6F9BADD0	E8 8B3DFFFF		CALL 6F9AEB60				; Restore code
6F9BADD5	8B04E4			MOV EAX,DWORD PTR SS:[ESP]		; \ Custom code
6F9BADD8	6A 20			PUSH 20					; |
6F9BADDA	FF90 BECC0000		CALL DWORD PTR DS:[EAX+0CCBE]		; |
6F9BADE0	C3			RETN					; /

; Enable Act V cinematics: add MPQ code (requires D2XVIDEO.MPQ)
6F9A78A2	A3 54C89F6F		MOV DWORD PTR DS:[6F9FC854],EAX		; CALL 6F9BADF0

6F9BADF0	3E:8B0CE4		MOV ECX,DWORD PTR DS:[ESP]		; \ Custom code
6F9BADF4	81C1 853C0100		ADD ECX,13C85				; |
6F9BADFA	89CA			MOV EDX,ECX				; |
6F9BADFC	81C2 903B0000		ADD EDX,3B90				; |
6F9BAE02	68 B80B0000		PUSH 0BB8				; |
6F9BAE07	56			PUSH ESI				; |
6F9BAE08	57			PUSH EDI				; |
6F9BAE09	57			PUSH EDI				; |
6F9BAE0A	52			PUSH EDX				; |
6F9BAE0B	832CE4 3C		SUB DWORD PTR SS:[ESP],3C		; |
6F9BAE0F	8982 98D70300		MOV DWORD PTR DS:[EDX+3D798],EAX	; |
6F9BAE15	E8 818BFFFF		CALL 6F9B399B				; |
6F9BAE1A	C3			RETN					; /

6FA6C6E2	A338			DW 38A3					; 0000 : Kill relocation

;**************************************************
; GAME.EXE
;**************************************************
; [EXTRA]
; Allow game to be ran without CD: .text unpacked and hidden calls to KERNEL32/USER32 fixed
