<h1>CpuPcStack</h1>
udp schnittstelle zwischen pc und cpu

<h2>Beschreibung</h2>
<p>
Dieses Tool ist eine Schnittstelle zwischen PCs und Siemens CPUs (12xx) via Ethernet.
Die Schnittstelle setzt auf udp/ip auf.
Ziel dieses Projekts ist es eine generische Schnittstelle zwischen PCs und CPUs zu realisieren.
</p>

<h2>PC Seite</h2>
<p>
In überlagerten Applikationen soll diese Schnittstelle einfach integrierbar sein.</br>
Referenz auf UdpPlcLIB anlegen</br>
Interface verwenden: UdpPlcLIB.IUdpPlcLib</br>
Instanz erstellen: UdpPlcLIB.net_udp net_udp = new UdpPlcLIB.net_udp(this);</br>
</p>

<h2>CPU Seite</h2>
<p>
Auf PC Seite ist die Schnittstelle als Windows.Forms .NET Applikation realisiert.
In der CPU gibt es zwei Global DBs (ein Empfangs- und ein Sendepuffer) die aus dem Anwenderprogramm angesprochen werden.
Im Ordner plc_sourcen sind die notwendigen CPU Programmbausteine als SCL Code hinterlegt.
Diese Dateien müssen mittels des Tools "TIA Portal" projektiert und in die CPU geladen werden.
</p>