   M S B A M L   `   `   `  ����O  KPictureViewer, Version=1.0.4084.18930, Culture=neutral, PublicKeyToken=null-clr-namespace:PictureViewerPictureViewer  5          PictureViewer.Window1   5   	   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5      +localclr-namespace:PictureViewer    5        � Title$  PictureViewer��5       � SizeToContent$ WidthAndHeight=�6   .��5      ]�  � 	Resources 5	      7  1System.Windows.Data.XmlNamespaceMappingCollection   mapping
 
� Uri 
� Prefix 
 Picasa � XmlNamespaceManager � Source��%6  &	        &	 �     7     5      
� 5   
   $! http://www.w3.org/2005/Atom?�6   $ default��6@    
� 5   
   $+ %http://schemas.google.com/photos/2007?�6   $ gphoto��6J   5   	   � 5      8   $H Bhttp://picasaweb.google.com/data/feed/api/user/rohits79?kind=album?�6V   5   	   5      .+�5      �� $
��Auto=�6   $
��Auto=�6C   .��5      �� -   5        $
��Left=�5       � DataContext# [ 6K   ��6r   ��  �� XPath$. (default:feed/default:entry/default:title��5   	   5      �� 5      -   5        	 ��Name$	 	MyListBox��5      $��Disabled=�6k   
���������6!   �� $1 +/default:feed/default:entry/default:content�� 5      � 5   
   # [  6   5      ��5      �� 5    
   .R�5!      �� $	��300��60   ��6   �� $
 @src��6>   5"      ��5$      �� 5%   
   .R�5&      � 5'      5)      5*      5+      