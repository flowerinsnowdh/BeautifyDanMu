package online.flowerinsnow.beautifydanmu.web;

import java.io.*;
import java.net.ServerSocket;
import java.net.Socket;
import java.nio.charset.StandardCharsets;

public class Main {
    private static StringBuilder danmuPool = new StringBuilder();
    public static void main(String[] args) {
        ServerSocket server;
        try {
            server = new ServerSocket(80);
        } catch (IOException e) {
            e.printStackTrace();
            return;
        }
        while (true) {
            Socket socket = null;
            BufferedReader br = null;
            PrintWriter pw = null;
            try {
                socket = server.accept();
                br = new BufferedReader(new InputStreamReader(socket.getInputStream(), StandardCharsets.UTF_8));
                String line = br.readLine();
                System.out.println(line);
                if (line.equals("GET / HTTP/1.1")) {
                    while ((line = br.readLine()) != null) {
                        System.out.println(line);
                        if (line.equals("")) break;
                    }
                    pw = new PrintWriter(new OutputStreamWriter(socket.getOutputStream(), StandardCharsets.UTF_8));
                    pw.println("HTTP/1.1 200");
                    pw.println("Content-Type: application/text;charset=utf-8");
                    pw.println("Access-Control Allow-Credentials: true");
                    pw.println("Access-Control-Allow-Origin: *");
                    pw.println();
                    pw.println(danmuPool.toString());
                    pw.flush();
                    danmuPool = new StringBuilder();
                } else if (line.equals("POSTDANMU")) {
                    System.out.println("POSTDANMU");
                    while ((line = br.readLine()) != null) {
                        danmuPool.append(line).append("\n");
                        System.out.println(line);
                    }
                }
            } catch (IOException e) {
                e.printStackTrace();
            } finally {
                closeQuietly(pw, br, socket);
            }
        }
    }

    private static void closeQuietly(AutoCloseable... acs) {
        for (AutoCloseable ac : acs) {
            if (ac != null) {
                try {
                    ac.close();
                } catch (Exception ignored) {
                }
            }
        }
    }
}
