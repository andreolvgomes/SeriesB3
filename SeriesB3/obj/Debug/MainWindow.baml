   M S B A M L   `   `   `  ����C  ?SeriesB3, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:SeriesB3.ConvertersSeriesB3.Converters  5   	   7 clr-namespace:SeriesB3.ExtendersSeriesB3.Extenders  5   	       SeriesB3.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   8
converters!clr-namespace:SeriesB3.Converters     5   	   6	extenders clr-namespace:SeriesB3.Extenders     5   	     � Title$  
MainWindow��5   	   $	��500��6    � WindowStartupLocation$ CenterScreen=�6(    � 
ResizeMode$ CanMinimize=�6M    � SizeToContent$ Height=�6f    � 	Resources 5      =   7SeriesB3.Converters.BooleanToVisibilityConverterReverse '  #BooleanToVisibilityConverterReverse   BooleanToVisibilityConverter��%:   &	        &	        5   
   69   �� 5   
   6'   5      .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   � {�6   �� *InProcessing+ �� 	Converter# [   5      �� 5      $
��Auto �6    �� 5      $��* �6    �� 5      $
��Auto �6    .+�5      �� .+�5      �� $��Séries Históricas b3��6   $��25(�68   $��Center=�6F   5      �� 5      $��1��6   $��0,10,0,0q�6&   .+�5       �  � ColumnDefinitions 5!      �� 5"      $��* �6+   �� 5#      $��* �6+   .+�5%      �� $(��"Arquivo com séries históricas b3��6    �� 5&      -   5          TextBlockLink#��[ 5&       $��Baixar��6G   $��1��6�   5'      � 5(      $��0,0,0,10q�6    5)      �� 5*      $��* �6+   �� 5+      $
��Auto �6+   .+�5.      �� -   5         ��Name$ txtFile��5.      	 �� 
IsReadOnly	 . 
 � ToolTip$#
 Arquivo baixado do site da b3��6~   ��6/   �� *FileB3+ �� UpdateSourceTrigger$ PropertyChanged=�6~   �� 5/      -   5        $��1��5/      $	��...��6-   $��30��6;   6F   50      �� 52      $)��#Filtrar por Código de Negociação��6   $[
 UMarque está opção para extrair dados somente dos Código de Negociação informado��6F    d� 	IsChecked 6�   �� *
ByCodneg+�� 53      -   5        $ txtAtivs��53      $��50��6,    �� TextWrapping$
 Wrap=�68    �� CharacterCasing$ Upper=�6L   $N
 HInforme o Código de Negociação delimitador por ';' que deseja filtrar��6�    �� AcceptsReturn . {�6d   �� *
ByCodneg+��54      �� *FilterCodneg+$ PropertyChanged=�6]   �� 56      $��0,5,0,0q�6   ��57      (   "SeriesB3.Extenders.RadioButtonEnum  58      -   5          Name$ rdToCsv��58   4   $	��CSV��6E    	� 	GroupName$ raSend��6S      	EnumValue$ ToCsv��6�   $
 TrueY�6�      EnumBinding 6f   �� *ToType+6�   .��5:      �� {�6!   ��  �� ElementName$ rdToCsv�� �� Path$ 	IsChecked�.+�5;      �� $:��4Gerar CSV separado para cada Código de Negocaição��6#    6_   �� *CsvSeparated+5<      5=      �� 5?      $��0,5,0,0q�6   ��5@        5A      -   5        $ radSql��5A   4   $��Microsoft SQL Server��6D   $ raSend��6c   $ ToSql��6�    6v   �� *ToType+6�   .��5D      �� {�6!   �� $ radSql��$ 	IsChecked�.+�5E      �  5F      �� 5G   "   $��* �63   �� 5H   "   $��* �63   .+�5J      �� $��Connection String��6(   �� 5K      -   5        $'��!Remover todas as tabelas do banco��5K   (   $��1��6Q   #��[ 6a   $/
 )Deleta todas as tabelas do banco de dados��6�   6�   5L      �� 5M      -   5        $ txtConnectionString��5M   "   ��6?   �� *ConnectionString+$ PropertyChanged=��� 5N      $+��%Limpa tabela antes de salvar os dados��6#   $��0,5,0,0q�6S   $?
 9Exclui todos os dados existentes antes de salvar os novos��6�    6d   �� *	DropTable+6�   �� 5O      $A��;Salvar em tabela separada para cada Código de Negociação��6#   $	��0,5q�5P      $e
 _Cria uma tabela para cada Código de Negociação e salva os dados separadamente em cada tabela��6-    5O   f   �� *IsTableSeparated+$ PropertyChanged=�5P   -   �� 5Q      $��Tabela��6$   �� 5R      -	   5        $ txtTable��5R   "   $8
 2Nome da tabela onde será salvo todos os registros��5S      ��5R   4   �� *Table+$ PropertyChanged=�{�6p   �� *IsTableSeparated+# [  5S      5T      5U      5V      �� 5X      $��2��6    �� Orientation$ 
Horizontal=�6&   $��Right=�6?   $��0,10,0,0q�6[   .+�5Y      �� -
   5        $��20��5Y      $	��250��6%   $��Gerar��61   6A   5Z      5[      �� 5]   
   $��1��6   $��Processando, aguarde ...��6!   $��15(�6A   
��� d �$
��Bold"�6f   y�6x   �� *InProcessing+# [ 5^      5_      