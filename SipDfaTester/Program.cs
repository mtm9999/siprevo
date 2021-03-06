﻿using System;
using System.Text;
using System.Threading;
using Sip.Message;

namespace SipDfaTester
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Write("Loading...");
			int start = Environment.TickCount;

			var dfaDummy = new SipMessageReader();

			var dfa = new SipMessageReader();
			//dfa.LoadTables(@"..\..\..\Sip.Message\SipMessageReader.dfa");
			dfa.LoadTables(@"..\..\..\SipDfaCompiler\bin\Debug\Sip.Message.dfa");
			int loadTablesDelay = Environment.TickCount - start;
			start = Environment.TickCount;
			dfa.SetDefaultValue();
			dfa.Parse(new byte[] { 0 }, 0, 1);
			Console.WriteLine("Done (LoadTables {0} ms + JIT {1} ms)", loadTablesDelay, Environment.TickCount - start);

#if OPTIMIZED2
			Console.WriteLine(dfa.Index);
			dfaDummy.Dispose();
#endif

			int proccessed = -1;
			var utf = new UTF8Encoding();

			var message0 = utf.GetBytes(
				"REGISTER sip:officesip.local;ms-received-cid=A123F;lrwrong;transport=tcp SIP/2.0\r\n" +
				"Accept: media/submedia\r\n" +
				"Via: SIP/2.0/WS 127.0.0.1:1800, SIP/2.0/TCP 127.0.0.2:1800, SIP/2.0/TCP 127.0.0.3:1800\r\n" +
				"Max-Forwards: 70\r\n" +
				"From: <sip:a@officesip.local;maddr=123.123.123.123>;tag=566ec054f8;epid=aaa6ef05f4\r\n" +
				"To: <sip:b@officesip.local>\r\n" +
				"Call-ID:    16743485a6e45407e903f75571a3a7af9\r\n" +
				"CSeq: 2 REGISTER\r\n" +
				"Contact: <sip:user@127.0.0.1:1801;transport=tcp;ms-opaque=0deb5c7e83>;methods=\"INVITE, MESSAGE, INFO, OPTIONS, BYE, CANCEL, NOTIFY, ACK, REFER, BENOTIFY\";proxy=replace  ; +sip.instance=\"<urn:uuid:92794BFE-F550-5AC1-89B4-F1E4E8BCB878>\";expires=123\r\n" +
				"Contact: <sip:127.0.0.2:1802;transport=tcp;ms-opaque=0deb5c7e83>;methods=\"INVITE, MESSAGE, INFO, OPTIONS, BYE, CANCEL, NOTIFY, ACK, REFER, BENOTIFY\";proxy=replace;+sip.instance=\"<urn:uuid:92794BFE-F550-5AC1-89B4-F1E4E8BCB878>\",<sip:127.0.0.3:1803;transport=tcp;ms-opaque=0deb5c7e83>;methods=\"INVITE\"\r\n" +
				"Contact: <sip:127.0.0.4:1804;transport=tcp;ms-opaque=0deb5c7e83>;methods=\"INVITE, MESSAGE, INFO, OPTIONS, BYE, CANCEL, NOTIFY, ACK, REFER, BENOTIFY\";proxy=replace;+sip.instance=\"<urn:uuid:92794BFE-F550-5AC1-89B4-F1E4E8BCB878>\"\r\n" +
				"Custom: Value\r\n" +
				"Authorization: Digest username=\"jdoe1\", realm=\"officesip.local\", qop=auth, algorithm=MD5, uri=\"sip:officesip.local\", nonce=\"50c148849f3d4e069105f8a80471b5d1\", nc=f1f, cnonce=\"53186537273641419345563711231350\", opaque=\"f2cb4f2013d74a73b9e86603fb56b969\", response=\"2bb45befa63ca772b840501502df102d\"\r\n" +
				"Authorization: Digest username=\"jdoe2\", realm=\"officesip.local\", qop=auth, algorithm=MD5, uri=\"sip:officesip.local\", nonce=\"50c148849f3d4e069105f8a80471b5d1\", nc=f2f, cnonce=\"53186537273641419345563711231350\", opaque=\"f2cb4f2013d74a73b9e86603fb56b969\", response=\"2bb45befa63ca772b840501502df102d\"\r\n" +
				"Authorization: Digest username=\"jdoe3\", realm=\"officesip.local\", qop=auth, algorithm=MD5, uri=\"sip:officesip.local\", nonce=\"50c148849f3d4e069105f8a80471b5d1\", nc=f3f, cnonce=\"53186537273641419345563711231350\", opaque=\"f2cb4f2013d74a73b9e86603fb56b969\", response=\"2bb45befa63ca772b840501502df102d\"\r\n" +
				"Record-Route: <sip:127.0.0.1:5060;lr> , <sip:127.0.0.2:5060;lr>\r\n" +
				"Route: <sip:127.0.0.1:5060;lr>, <sip:127.0.0.2:5060;lr>\r\n" +
				"Route: <sip:127.0.0.3:5060;lr>\r\n" +
				"Event: presence\r\n" +
				"Content-Type: application / msrtc-adrl-categorylist+xml\r\n" +
				"\r\n");

			//////var message0 = utf.GetBytes(
			//////    "REGISTER sip:officesip.local;ms-received-cid=A123F;lrwrong;transport=tcp SIP/2.0\r\n" +
			//////    "Authorization: Digest username=\"jdoe1\", realm=\"officesip.local\", qop=auth, algorithm=MD5, uri=\"sip:officesip.local\", nonce=\"50c148849f3d4e069105f8a80471b5d1\", nc=f1f, cnonce=\"53186537273641419345563711231350\", opaque=\"f2cb4f2013d74a73b9e86603fb56b969\", response=\"2bb45befa63ca772b840501502df102d\"\r\n" +
			//////    "Authorization: Digest username=\"jdoe2\", realm=\"officesip.local\", qop=auth, algorithm=MD5, uri=\"sip:officesip.local\", nonce=\"50c148849f3d4e069105f8a80471b5d1\", nc=00000f2f, cnonce=\"53186537273641419345563711231350\", opaque=\"f2cb4f2013d74a73b9e86603fb56b969\", response=\"2bb45befa63ca772b840501502df102d\"\r\n" +
			//////    "Authorization: Digest username=\"jdoe3\", realm=\"officesip.local\", qop=auth, algorithm=MD5, uri=\"sip:officesip.local\", nonce=\"50c148849f3d4e069105f8a80471b5d1\", nc=0000f3f, cnonce=\"53186537273641419345563711231350\", opaque=\"f2cb4f2013d74a73b9e86603fb56b969\", response=\"2bb45befa63ca772b840501502df102d\"\r\n" +
			//////    "Proxy-Authorization: Digest username=\"jdoe4\", realm=\"officesip.local\", qop=auth, algorithm=MD5, uri=\"sip:officesip.local\", nonce=\"50c148849f3d4e069105f8a80471b5d1\", nc=f4f, cnonce=\"53186537273641419345563711231350\", opaque=\"f2cb4f2013d74a73b9e86603fb56b969\", response=\"2bb45befa63ca772b840501502df102d\"\r\n" +
			//////    "Proxy-Authorization: Digest username=\"jdoe5\", realm=\"officesip.local\", qop=auth, algorithm=MD5, uri=\"sip:officesip.local\", nonce=\"50c148849f3d4e069105f8a80471b5d1\", nc=00000f5f, cnonce=\"53186537273641419345563711231350\", opaque=\"f2cb4f2013d74a73b9e86603fb56b969\", response=\"2bb45befa63ca772b840501502df102d\"\r\n" +
			//////    "Proxy-Authorization: Digest username=\"jdoe6\", realm=\"officesip.local\", qop=auth, algorithm=MD5, uri=\"sip:officesip.local\", nonce=\"50c148849f3d4e069105f8a80471b5d1\", nc=0000f6f, cnonce=\"53186537273641419345563711231350\", opaque=\"f2cb4f2013d74a73b9e86603fb56b969\", response=\"2bb45befa63ca772b840501502df102d\"\r\n" +
			//////    "\r\n");

			var message1 = new byte[message0.Length + 16];
			Buffer.BlockCopy(message0, 0, message1, 8, message0.Length);

			dfa.SetDefaultValue();

			proccessed = dfa.Parse(message1, 8, message1.Length);

			dfa.SetArray(message1);

			if (dfa.Error)
				WriteError(proccessed, message1);

			Console.WriteLine("Total: {0}", message1.Length);
			Console.WriteLine("Proccessed: {0}", proccessed);
			Console.WriteLine("Final: {0}", dfa.Final);
			Console.WriteLine("Method: {0}", dfa.Method);
			Console.WriteLine("SipVersion: {0}", dfa.SipVersion);

			Console.WriteLine();
			Console.WriteLine("Headers: {0}", dfa.Count.HeaderCount);
			for (int i = 0; i < dfa.Count.HeaderCount; i++)
				Console.WriteLine("{0}-|{1}| : |{2}|", dfa.Headers[i].HeaderName, dfa.Headers[i].Name.ToString(), dfa.Headers[i].Value.ToString());
			Console.WriteLine();

			//////Console.WriteLine("AuthCount: {0}", dfa.Count.AuthorizationCount);

			//////Console.WriteLine("Auth #0: {0}, {1}", dfa.Authorization[0].Username.ToString(), dfa.Authorization[0].NonceCountBytes.ToString());
			//////Console.WriteLine("Auth #1: {0}, {1}", dfa.Authorization[1].Username.ToString(), dfa.Authorization[1].NonceCountBytes.ToString());
			//////Console.WriteLine("Auth #2: {0}, {1}", dfa.Authorization[2].Username.ToString(), dfa.Authorization[2].NonceCountBytes.ToString());

			//////Console.WriteLine("ProxyAuthCount: {0}", dfa.Count.ProxyAuthorizationCount);

			//////Console.WriteLine("ProxyAuth #0: {0}, {1}", dfa.ProxyAuthorization[0].Username.ToString(), dfa.ProxyAuthorization[0].NonceCountBytes.ToString());
			//////Console.WriteLine("ProxyAuth #1: {0}, {1}", dfa.ProxyAuthorization[1].Username.ToString(), dfa.ProxyAuthorization[1].NonceCountBytes.ToString());
			//////Console.WriteLine("ProxyAuth #2: {0}, {1}", dfa.ProxyAuthorization[2].Username.ToString(), dfa.ProxyAuthorization[2].NonceCountBytes.ToString());

			//Console.WriteLine("RequestUri: {0}", dfa.RequestUri.Value.ToString());
			////Console.WriteLine("RequestUri.MsReceivedCid: {0}", dfa.RequestUri.MsReceivedCid.ToString());
			////Console.WriteLine("RequestUri.Transport: {0}", dfa.RequestUri.Transport.ToString());
			//Console.WriteLine("Header #0: {0}", dfa.Headers[0].HeaderName.ToString());
			////Console.WriteLine("Proxy Replace #0: |{0}|", dfa.Contact[0].ProxyReplace.ToString());
			////Console.WriteLine("Proxy Replace #1: |{0}|", dfa.Contact[1].ProxyReplace.ToString());
			////Console.WriteLine("Proxy Replace #2: |{0}|", dfa.Contact[2].ProxyReplace.ToString());
			////Console.WriteLine("Proxy Replace #3: |{0}|", dfa.Contact[3].ProxyReplace.ToString());
			////Console.WriteLine("User #0: |{0}|", dfa.Contact[0].AddrSpec2.User.ToString());
			//Console.WriteLine("AddrSpec #0: |{0}| : {1}", dfa.Contact[0].AddrSpec2.Hostport.Host.ToString(), dfa.Contact[0].AddrSpec2.Hostport.Port);
			//Console.WriteLine("AddrSpec #1: |{0}| : {1}", dfa.Contact[1].AddrSpec2.Hostport.Host.ToString(), dfa.Contact[1].AddrSpec2.Hostport.Port);
			//Console.WriteLine("AddrSpec #2: |{0}| : {1}", dfa.Contact[2].AddrSpec2.Hostport.Host.ToString(), dfa.Contact[2].AddrSpec2.Hostport.Port);
			//Console.WriteLine("AddrSpec #3: |{0}| : {1}", dfa.Contact[3].AddrSpec2.Hostport.Host.ToString(), dfa.Contact[3].AddrSpec2.Hostport.Port);

			//Console.WriteLine("RequestUri: |{0}|", dfa.RequestUri.Hostport.Host.ToString());
			//Console.WriteLine("ContactCount: {0}", dfa.Count.ContactCount);
			//Console.WriteLine("AddrSpec #0: |{0}| : {1}", dfa.Contact[0].AddrSpec2.Hostport.Host.ToString(), dfa.Contact[0].AddrSpec2.Hostport.Port);
			//Console.WriteLine("AddrSpec #1: |{0}| : {1}", dfa.Contact[1].AddrSpec2.Hostport.Host.ToString(), dfa.Contact[1].AddrSpec2.Hostport.Port);
			//Console.WriteLine("AddrSpec #2: |{0}| : {1}", dfa.Contact[2].AddrSpec2.Hostport.Host.ToString(), dfa.Contact[2].AddrSpec2.Hostport.Port);
			//Console.WriteLine("AddrSpec #3: |{0}| : {1}", dfa.Contact[3].AddrSpec2.Hostport.Host.ToString(), dfa.Contact[3].AddrSpec2.Hostport.Port);
			//Console.WriteLine("Expires 1: {0}", dfa.Contact[0].Expires);
			//Console.WriteLine("Expires 2: {0}", dfa.Contact[1].Expires);
			//Console.WriteLine("Star: {0}", dfa.Contact[0].IsStar);
			//Console.WriteLine("Via: {2} = {0} : {1}", dfa.Via[0].SentBy.Host.ToString(), dfa.Via[0].SentBy.Port, dfa.Via[0].Transport.ToString());
			//Console.WriteLine("Via: {2} = {0} : {1}", dfa.Via[1].SentBy.Host.ToString(), dfa.Via[1].SentBy.Port, dfa.Via[1].Transport.ToString());
			//Console.WriteLine("CSeq: {0} {1}", dfa.CSeq.Value, dfa.CSeq.Method);
			//Console.WriteLine("MaxForwards: {0}", dfa.MaxForwards);
			//Console.WriteLine("From: {0} : {1} @ {2} ; Maddr= {3}", dfa.From.AddrSpec1.UriScheme.ToString(), dfa.From.AddrSpec1.User.ToString(), dfa.From.AddrSpec1.Hostport.Host.ToString(), dfa.From.AddrSpec1.Maddr.ToString());
			//Console.WriteLine("To: {0}", dfa.To.AddrSpec1.Hostport.Host.ToString());
			//Console.WriteLine("CallId: |{0}|", dfa.CallId.ToString());
			//Console.WriteLine("Record-Route[0].CommaAndValue: |{0}|", dfa.RecordRoute[0].CommaAndValue.ToString());
			//Console.WriteLine("Record-Route[1].CommaAndValue: |{0}|", dfa.RecordRoute[1].CommaAndValue.ToString());
			//Console.WriteLine("Route[0].CommaAndValue: |{0}|", dfa.Route[0].CommaAndValue.ToString());
			//Console.WriteLine("Route[1].CommaAndValue: |{0}|", dfa.Route[1].CommaAndValue.ToString());
			//Console.WriteLine("Route[2].CommaAndValue: |{0}|", dfa.Route[2].CommaAndValue.ToString());
			//Console.WriteLine("Via[0].CommaAndValue: |{0}|", dfa.Via[0].CommaAndValue.ToString());
			//Console.WriteLine("Via[1].CommaAndValue: |{0}|", dfa.Via[1].CommaAndValue.ToString());
			//Console.WriteLine("Via[0].ValueAndComma: |{0}|", dfa.Via[0].ValueAndComma.ToString());
			//Console.WriteLine("Via[1].ValueAndComma: |{0}|", dfa.Via[1].ValueAndComma.ToString());
			//Console.WriteLine("Via[0].Value: |{0}|", dfa.Via[0].Value.ToString());
			//Console.WriteLine("Via[1].Value: |{0}|", dfa.Via[1].Value.ToString());
			//Console.WriteLine("HasLr: |{0}|", dfa.RequestUri.HasLr);
			//Console.WriteLine("Event: |{0}|", dfa.Event.EventType.ToString());
			//Console.WriteLine("Content-Type.Type: |{0}|", dfa.ContentType.Type.ToString());
			//Console.WriteLine("Content-Type.Subtype: |{0}|", dfa.ContentType.Subtype.ToString());

			//Console.WriteLine();
			//Console.WriteLine("AUTHORIZATION:");
			//Console.WriteLine("AuthScheme: {0}", dfa.Authorization.AuthScheme);
			//Console.WriteLine("AuthAlgorithm: {0}", dfa.Authorization.AuthAlgorithm);
			//Console.WriteLine("Nonce: |{0}|", dfa.Authorization.Nonce.ToString());
			//Console.WriteLine("Username: |{0}|", dfa.Authorization.Username.ToString());
			//Console.WriteLine("Realm: |{0}|", dfa.Authorization.Realm.ToString());
			//Console.WriteLine("MessageQop: |{0}|", dfa.Authorization.MessageQop.ToString());
			//Console.WriteLine("Cnonce: |{0}|", dfa.Authorization.Cnonce.ToString());
			//Console.WriteLine("NonceCount: {0}", dfa.Authorization.NonceCount);
			//Console.WriteLine("Opaque: |{0}|", dfa.Authorization.Opaque.ToString());
			//Console.WriteLine("MessageQop: |{0}|", dfa.Authorization.MessageQop.ToString());
			//Console.WriteLine("Response: |{0}|", dfa.Authorization.Response.ToString());
			//Console.WriteLine();

			////Console.WriteLine("Headers: {0}", dfa.Count.HeaderCount);
			////for (int i = 0; i < dfa.Count.HeaderCount; i++)
			////    Console.WriteLine("{0}:\r\n|{1}|", dfa.Headers[i].Name.ToString(), dfa.Headers[i].Value.ToString());

			var message2 = utf.GetBytes(
				"BENOTIFY sip:user:password@officesip.local:5060 SIP/2.0\r\n" +
				"Contactz: <sip:domain>;proxy=replace  ; next=param;next=param\r\n" +
				"Contactz: <sip:domain>;  proxy=replace,<sip:domain>;proxy=replace\r\n" +
				"Contactz: <sip:domain>;proxy=replace\r\n" +
				"Contactz: <sip:domain>;proxy=replacewrong\r\n" +
				//		"Via: SIP/2.0/TCP 127.0.0.1:1800;ms-received-cid=ABC0  ;ms-received-port=12345 \r\n" +
				//		"Contact: *\r\n" +
				//		"Require: token1, token2, token3\r\n" +
				//		"Require: token4\r\n" +
				//		"Proxy-Require: token1, token2, token3\r\n" +
				"\r\n");

			dfa.SetDefaultValue();
			proccessed = dfa.Parse(message2, 0, message2.Length);
			dfa.SetArray(message2);

			if (dfa.Error)
				WriteError(proccessed, message2);

			Console.WriteLine("--");
			Console.WriteLine("Total: {0}", message2.Length);
			Console.WriteLine("Proccessed: {0}", proccessed);
			Console.WriteLine("Final: {0}", dfa.Final);
			Console.WriteLine("Method: {0}", dfa.Method);
			//Console.WriteLine("ContactCount: {0}", dfa.Count.ContactCount);
			//Console.WriteLine("RequireCount: {0}", dfa.Count.RequireCount);
			//Console.Write("Require: ");
			//for (int i = 0; i <= dfa.Count.RequireCount; i++)
			//    Console.Write("{0}, ", dfa.Require[i].ToString());
			//Console.WriteLine();
			//Console.Write("Proxy-Require: ");
			//for (int i = 0; i <= dfa.Count.ProxyRequireCount; i++)
			//    Console.Write("{0}, ", dfa.ProxyRequire[i].ToString());
			Console.WriteLine();
			//Console.WriteLine("Star: {0}", dfa.Contact[0].IsStar);
			//Console.WriteLine("RequestUri.User: |{0}|", dfa.RequestUri.User.ToString());
			//Console.WriteLine("RequestUri.Hostport.Host: |{0}|", dfa.RequestUri.Hostport.Host.ToString());
			//Console.WriteLine("Via[0].MsReceived.Port: |{0}|", dfa.Via[0].MsReceived.Port);
			//Console.WriteLine("Via[0].MsReceived.Cid: |{0}|", dfa.Via[0].MsReceived.Cid.ToString());

			//Console.WriteLine("ContactCount: {0}", dfa.Count.ContactCount);
			//Console.WriteLine("Proxy Replace #0: |{0}|", dfa.Contact[0].ProxyReplace.ToString());
			//Console.WriteLine("Proxy Replace #1: |{0}|", dfa.Contact[1].ProxyReplace.ToString());
			//Console.WriteLine("Proxy Replace #2: |{0}|", dfa.Contact[2].ProxyReplace.ToString());
			//Console.WriteLine("Proxy Replace #3: |{0}|", dfa.Contact[3].ProxyReplace.ToString());
			//Console.WriteLine("Proxy Replace #4: |{0}|", dfa.Contact[4].ProxyReplace.ToString());

			//var message3X = utf.GetBytes(
			//    "REGISTER sip:officesip.local SIP/2.0\r\n" +
			//    "Via: SIP/2.0/TCP 127.0.0.1:2260\r\n" +
			//    "Max-Forwards: 70\r\n" +
			//    "From: <sip:demchenko@officesip.local>;tag=9c0e7f8549;epid=fc68f4ccf1\r\n" +
			//    "To: <sip:demchenko@officesip.local>\r\n" +
			//    "Call-ID: 78cda9b3461a453e916f6ac7c7560198\r\n" +
			//    "CSeq: 1 REGISTER\r\n" +
			//    "Contact: <sip:127.0.0.1:2260;transport=tcp;ms-opaque=e24420fcd5>;methods=\"INVITE, MESSAGE, INFO, OPTIONS, BYE, CANCEL, NOTIFY, ACK, REFER, BENOTIFY\";proxy=replace;+sip.instance=\"<urn:uuid:02C51493-0E21-59BD-83E4-28EA883A80DA>\"\r\n" +
			//    "User-Agent: UCCAPI/2.0.6362.67\r\n" +
			//    "Supported: gruu-10, adhoclist, msrtc-event-categories\r\n" +
			//    "Supported: ms-forking\r\n" +
			//    "ms-keep-alive: UAC;hop-hop=yes\r\n" +
			//    "Event: registration\r\n" +
			//    "Content-Length: 0\r\n" +
			//    "\r\n");

			//if (message3X.Length != 0x0000029d)
			//    throw new Exception();

			//var message3 = new byte[0x10000000];
			//var offset3 = 0x00020000;

			//Buffer.BlockCopy(message3X, 0, message3, offset3, message3X.Length);

			//dfa.SetDefaultValue();
			//dfa.SetArray(message3);

			//Console.WriteLine("--");

			//proccessed = dfa.Parse(message3, offset3, message3X.Length) - offset3;

			//Console.WriteLine("Total: {0}", message3.Length);
			//Console.WriteLine("Proccessed: {0} ({1})", proccessed, (proccessed == message3X.Length) ? "Ok" : "Error");
			//Console.WriteLine("Final: {0}", dfa.Final);
			//Console.WriteLine("Method: {0}", dfa.Method);

			//var message4 = utf.GetBytes(
			//    "REGISTER sip:officesip.local SIP/2.0\r\n" +
			//    "Via: SIP/2.0/TCP 127.0.0.1:4470\r\n" +
			//    "Max-Forwards: 70\r\n" +
			//    "From: <sip:demchenko@officesip.local>;tag=69a9527323;epid=d270caa80e\r\n" +
			//    "To: <sip:demchenko@officesip.local>\r\n" +
			//    "Call-ID: 851c4f8aae634461af835fa8be6601da\r\n" +
			//    "CSeq: 3 REGISTER\r\n" +
			//    "Contact: <sip:127.0.0.1:4470;transport=tcp;ms-opaque=db2aa87295>;methods=\"INVITE, MESSAGE, INFO, OPTIONS, BYE, CANCEL, NOTIFY, ACK, REFER, BENOTIFY\";proxy=replace;+sip.instance=\"<urn:uuid:8F3D722D-B41C-518C-8FCD-159F96EA017E>\"\r\n" +
			//    "User-Agent: UCCAPI/2.0.6362.67\r\n" +
			//    "Authorization: NTLM qop=\"auth\", realm=\"OfficeSIP Server\", targetname=\"officesip.local\", gssapi-data=\"\", version=3\r\n" +
			//    "Supported: gruu-10, adhoclist, msrtc-event-categories\r\n" +
			//    "Supported: ms-forking\r\n" +
			//    "ms-keep-alive: UAC;hop-hop=yes\r\n" +
			//    "Event: registration\r\n" +
			//    "Content-Length: 0\r\n" +
			//    "\r\n");

			//dfa.SetDefaultValue();
			//dfa.SetArray(message4);
			//dfa.Parse(message4, 0, message4.Length);

			//Console.WriteLine("--");
			//Console.WriteLine("Final: {0}", dfa.Final);
			//Console.WriteLine("Authorization.MessageQop: {0}", dfa.Authorization.MessageQop.ToString());
			//Console.WriteLine("Authorization.GssapiData: {0}", dfa.Authorization.GssapiData.IsEmpty ? "NULL" : dfa.Authorization.GssapiData.ToString());

			var message5 = utf.GetBytes(
				"ACK sip:service@192.168.1.15:5060 SIP/2.0\r\n" +
				"Via: SIP/2.0/TCP [fe80::7006:2c9e:ab6b:b8fd]:5060;branch=z9hG4bK-5216-32-0\r\n" +
				"From: sipp <sip:sipp@[fe80::7006:2c9e:ab6b:b8fd]:5060>;tag=5216SIPpTag0032\r\n" +
				"To: sut <sip:service@192.168.1.15:5060>;tag=c58874cb6fe149b6ba70558a4050e8a9\r\n" +
				"Call-ID: 32-5216@fe80::7006:2c9e:ab6b:b8fd\r\n" +
				"CSeq: 1 ACK\r\n" +
				"Contact: <sip:sipp@[fe80::7006:2c9e:ab6b:b8fd]:5060;transport=TCP>\r\n" +
				"Max-Forwards: 70\r\n" +
				"Subject: Performance Test\r\n" +
				"Content-Length: 0\r\n" +
				"\r\n");

			dfa.SetDefaultValue();
			dfa.Parse(message5, 0, message5.Length);
			dfa.SetArray(message5);

			Console.WriteLine("--");
			Console.WriteLine("Final: {0}", dfa.Final);
			//Console.WriteLine("From.AddrSpec1 #0: |{0}|", dfa.From.AddrSpec1.Hostport.Host.ToString());
			//Console.WriteLine("From.AddrSpec2 #0: |{0}|", dfa.From.AddrSpec2.Hostport.Host.ToString());
			//Console.WriteLine("From.Tag #0: |{0}|", dfa.From.Tag.ToString());

			//var message6 = utf.GetBytes(
			//    "INVITE sip:rtk@13.141.225.46 SIP/2.0\r\n" +
			//    "From: sip:jupiter@xstar.local;tag=34f0f5fb-5586-48da-9d20-b23b2dc81ed5\r\n" +
			//    "\r\n");


			//dfa.SetDefaultValue();
			//dfa.Parse(message6, 0, message6.Length);
			//dfa.SetArray(message6);

			//Console.WriteLine("Final: {0}", dfa.Final);
			//Console.WriteLine("From: {0} : {1} @ {2} ; tag= {3}", dfa.From.AddrSpec2.UriScheme.ToString(),
			//    dfa.From.AddrSpec2.User.ToString(), dfa.From.AddrSpec2.Hostport.Host.ToString(),
			//    dfa.From.Tag.ToString());

			//Console.WriteLine("--");

			//var message7 = utf.GetBytes("REGISTER sip:officesip.local SIP/2.0\r\n\r\nR");
			//dfa.SetDefaultValue();
			//int parsed7 = dfa.Parse(message7, 0, message7.Length);
			//Console.WriteLine("Final : {0}", dfa.Final);
			//Console.WriteLine("Parsed: {0} ({1})", parsed7, message7.Length);

			Console.WriteLine("--");

			var message8 = utf.GetBytes(
				"REGISTER sip:officesip.local SIP/2.0\r\n" +
				"Accept: image/text1\r\n" +
				"Accept: application/text2\r\n" +
				"Content-Type:    application/text2\r\n" +
				"Via: SIP/2.0/TCP 127.0.0.1:1800,    SIP/2.0/TCP 127.0.0.2:1800\r\n" +
				"Via:     SIP/2.0/UDP 127.0.0.3:1800,    SIP/2.0/TCP 127.0.0.4:1800\r\n" +
				//"Authorization: Kerberos qop=\"auth\", realm=\"OfficeSIP Server\", opaque=\"028da4af\", targetname=\"sip/officesip.local\", version=3, gssapi-data=\"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" +
				//"Authorization: NTLM qop=\"auth\", realm=\"OfficeSIP Server\", opaque=\"05cee767\", targetname=\"officesip.local\", crand=\"3c1c4c7b\", cnum=\"1234567\", response=\"0100000061646d69de3324fd36334466\"\r\n" +
				//	Authorization: Digest username="proxy", realm="officesip.local", qop=auth, algorithm=MD5, uri="sip:officesip.local", nonce="d44b4f96b00841f9cada22d5cc678d94", nc=1, cnonce="0084800a", opaque="00000001", response="c07d8fc21bcf4ced9b45414f3d10a498"
				//"WWW-Authenticate: Digest realm=\"o1fficesip.local\",nonce=\"189396c747dedfc87fbe5deb420db5ca\",qop=\"auth\",algorithm=MD5,stale=false,opaque=\"abc00000001\"\r\n" +
				//"WWW-Authenticate: Digest realm=\"o2fficesip.local\",nonce=\"289396c747dedfc87fbe5deb420db5ca\",qop=\"auth\",algorithm=MD5,stale=true,opaque=\"def00000001\"\r\n" +
				//"WWW-Authenticate: Digest realm=\"of3ficesip.local\",nonce=\"389396c747dedfc87fbe5deb420db5ca\",qop=\"auth,auth-int\",algorithm=MD5,stale=false,opaque=\"gfh00000001\"\r\n" +
				//"Proxy-Authenticate: Digest realm=\"of3ficesip.local\",nonce=\"389396c747dedfc87fbe5deb420db5ca\",qop=\"auth,auth-int\",algorithm=MD5,stale=false,opaque=\"gfh00000001\"\r\n" +
				//"Proxy-Authenticate: Digest realm=\"of3ficesip.local\",nonce=\"389396c747dedfc87fbe5deb420db5ca\",qop=\"auth,auth-int\",algorithm=MD5,stale=false,opaque=\"gfh00000001\"\r\n" +
				"\r\n"
				);

			//discrete - type = "text" / "image" / "audio" / "video"
			//                    / "application" / extension - token;
			//composite - type = "message" / "multipart" / extension - token;

			dfa.SetDefaultValue();
			int parsed8 = dfa.Parse(message8, 0, message8.Length);
			dfa.SetArray(message8);
			Console.WriteLine("Final : {0}", dfa.Final);
			Console.WriteLine("Parsed: {0} ({1})", parsed8, message8.Length);

			for (int i = 0; i < dfa.Count.AcceptCount + 1; i++)
			{
				//Console.WriteLine("Accept: {0} {1}/{2}", dfa.Accept[i].MediaType.ToString(), dfa.Accept[i].Type.ToString(), dfa.Accept[i].Subtype.ToString());
				Console.WriteLine("Accept: |{0}| {1}/{2}", dfa.Accept[i].Value.ToString(), dfa.Accept[i].Type.ToString(), dfa.Accept[i].Subtype.ToString());
			}

			//Console.WriteLine("Content-Type: |{0}|", dfa.ContentType.Value.ToString());

			//for (int i = 0; i < dfa.Count.ViaCount + 1; i++)
			//    Console.WriteLine("Via.ValueAndComma: |{0}|", dfa.Via[i].ValueAndComma.ToString());

			//Console.WriteLine("www Count: {0}", dfa.Count.WwwAuthenticateCount);
			//Console.WriteLine("proxy Count: {0}", dfa.Count.ProxyAuthenticateCount);

			//for (int i = 0; i < dfa.Count.WwwAuthenticateCount + 1; i++)
			//{
			//    var www = dfa.WwwAuthenticate[i];

			//    Console.WriteLine();
			//    Console.WriteLine("Scheme: {0}", www.AuthScheme);
			//    Console.WriteLine("Nonce: {0}", www.Nonce.ToString());
			//    Console.WriteLine("Realm: {0}", www.Realm.ToString());
			//    Console.WriteLine("Qop: {0}", www.Qop.ToString());
			//    Console.WriteLine("Alg: {0}", www.AuthAlgorithm.ToString());
			//    Console.WriteLine("Opaque: {0}", www.Opaque.ToString());
			//    Console.WriteLine("Stale: {0}", www.Stale);
			//}

			//Console.WriteLine("Scheme: {0}", dfa.Authorization[0].AuthScheme);
			//Console.WriteLine("targetname: {0}", dfa.Authorization[0].Targetname.ToString());
			//Console.WriteLine("Crand: {0:x8}", dfa.Authorization[0].Crand);
			//Console.WriteLine("Cnum: {0}", dfa.Authorization[0].Cnum);
			//Console.WriteLine("Response: {0}", dfa.Authorization[0].Response.ToString());

			Console.WriteLine("--");

			//Console.ReadKey(true);

			Console.WriteLine("--");

			var message9 = utf.GetBytes("\r\n\r\n");
			dfa.SetDefaultValue();
			int parsed9 = dfa.Parse(message9, 0, message9.Length);
			Console.WriteLine("Parsed for \\r\\n\\r\\n: {0}", parsed9);

			var message10 = utf.GetBytes("REG@@@@");

			dfa.SetDefaultValue();
			int parsed10 = dfa.Parse(message10, 0, message10.Length);
			Console.WriteLine("Parsed for REG@@@@: {0}", parsed10);

			dfa.SetDefaultValue();
			int parsed11 = dfa.Parse(message10, 0, 4);
			Console.WriteLine("Parsed for REG@: {0}", parsed11);

			Console.WriteLine("Testing speed");

			int repeat = 100000;
			int start2 = Environment.TickCount;
			for (int i = 0; i < repeat; i++)
			{
				dfa.SetDefaultValue();
				dfa.Parse(message1, 0, message1.Length);
				dfa.SetArray(message1);
			}
			int spent = Environment.TickCount - start2;

			Console.WriteLine("Parsed {0} times, {1} ms", repeat, spent);
		}

		private static void WriteError(int proccessed, byte[] message2)
		{
			Console.WriteLine(Encoding.UTF8.GetString(message2, proccessed, Math.Min(100, message2.Length - proccessed)));
		}
	}
}
