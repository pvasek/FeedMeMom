﻿<?phpif(isset($_POST['submit'])) {   $to = 'webadmin@feedmemom.com' ;     //put your email address on which you want to receive the information   $subject = 'subscription';   //set the subject of email.   $headers  = "MIME-Version: 1.0\r\n";   $headers .= "Content-type: text/html; charset=utf-8\r\n";   $message = "E-Mail".$_POST['email'];   mail($to, $subject, $message, $headers);}$url = isset($_SERVER['HTTP_REFERER']) ? $_SERVER['HTTP_REFERER'] : '';if(preg_match('#(http?|ftp)://\S+[^\s.,>)\];\'\"!?]#i',$url)){    header("Location: $url");    echo "<html><head><meta http-equiv=\"refresh\" content=\"0;url=$url\"></head></html>";    exit();}?>