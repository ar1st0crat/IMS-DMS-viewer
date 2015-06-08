[English]
> The IMS-DMS-Viewer program is intended for analytical chemists and data-mining specialists in respective field. The main purpose of this tool is visualiazing, visual analyzing and comparing ion mobility spectra (IMS and DMS) of various chemical compounds. Repository also contains the python script for automatic finding of local peaks in DMS spectra.

Main features of the program are:
- automatic loading and parsing of all csv-files with DMS spectral data and measurement parameters from any given directory on the researcher's computer (the "Add Directory..." menu entry). The program also automatically finds and parses all files in the first level subfolders of the given folder.
- automatic removing of the "Sign Bit Artifact" in DMS spectra
- visualization of DMS-spectra and measurement parameters in the listview and charts with choice options for the chart style (line plot, bar plot, point plot)
- the capability of choosing the custom combination of spectra from the entire set of spectra
- automatic parsing of measurement parameters and scale fitting while simultaneous work with several spectra
- visualization of IMS chromatograms(the program works with the files of mzXml format)

![pic1](https://github.com/ar1st0crat/IMS-DMS-viewer/blob/master/screenshots/1.png)
![pic2](https://github.com/ar1st0crat/IMS-DMS-viewer/blob/master/screenshots/2.png)

<br/>
[Russian]<br/>
> DIMSS
Программа служит для визуализации и исследования спектров ионной мобильности (IMS-хроматограмм и DMS).

Возможности программы:
- автоматическая загрузка и разбор всех csv-файлов со спектральными данными ДМС и параметрами измерений из любого указанного каталога на компьютере исследователя (пункт меню Add Directory...). При этом программа подхватывает все файлы и из его вложенных подкаталогов;
- автоматическое удаление «артефактов знакового бита» в спектрах;
- отображение ДМС-спектров и параметров измерений в списке и на графиках с возможностью выбора одного из трех типов отображения графика (линии, закраска, точки);
- возможность выбора произвольной комбинации спектров из всего набора;
- автоматический разбор параметров измерений и подгонка шкалы при одновременном отображении нескольких спектров;
- возможность просмотра ИМС-хроматограмм (программа работает с файлами в формате mzXml).


> dims_peaks.py
Скрипт на Python, позволяющий детектировать пики в ДМС-спектрах.

Визуальный анализ различных ДМС-спектров веществ при различных условиях измерений позволил выделить три основных типа пиков:
1) spikes ("острые" пики в один или несколько отсчетов дискретного спектра при заданной дискретизации спектра; амплитуда значительно отличается от амплитуды соседних отсчетов);
2) plateau (клиппированные, обрезанные по уровню участки спектра; наличие таких пиков вызвано высокой чувствительностью ДИМС-спектрометра);
3) hills (традиционные гладкие пики – участки спектра, которые можно в дальнейшем аппроксимировать гауссоидами, лоренцианами и подобными функциями).
Разработан алгоритм первичного обнаружения пиков в спектре (применяется термин "первичное", т.к. на этапе аппроксимации спектра в области пиков их характеристики могут быть уточнены). Для идентификации пиков анализируются сглаженные кривые производных сигнала первого и второго порядков. Для сглаживания применяется фильтр Савицкого-Голея, медианный фильтр и скользящее среднее. Для идентификации пиков 1 и 3 типа анализируются точки, в которых кривая производной меняет знак (если расстояние между этими точками – «временными» пиками – достаточно мало, то из них выбирается точка с самой большой амплитудой; если разница больше некоторого порога, то пик помечается как spike).
Для идентификации пиков 2 типа анализируются точки, в которых значение первой и второй производной равно 0, а значение отсчетов самого спектра близко к максимальному уровню динамического диапазона. Эксперименты показали, что тривиальное сравнение амплитуды с максимально возможной при заданном уровне квантования (|Amax| = 32768 при 16-разрядном АЦП) не применимо, т.к. клиппирование может иметь место и на меньших амплитудах. В связи с этим, сначала алгоритм анализирует гистограмму спектра и находит выбросы в верхнем и нижнем децилях. Затем все подряд идущие отсчеты спектра с соответствующей амплитудой помечаются как “clips”. 
Свободными параметрами разработанного алгоритма являются:
•	размер ядра скользящего среднего (выставлено MA_size = 15);
•	максимально допустимое расстояние между «временными» пиками (выставлено distanceThreshold = 17).
