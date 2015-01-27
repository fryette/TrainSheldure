﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App3.Infrastructure;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace App3
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage2 : Page
    {

        public BlankPage2()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        public List<string> AutoCompletions = new List<string>()
        {
            #region cityes
            
            "11 км",
            "143 км",
            "153 км",
            "15 км",
            "164 км",
            "187 км",
            "19 км",
            "202 км",
            "221 км",
            "238 км",
            "25 км",
            "314 км",
            "323 км",
            "331 км",
            "336 км",
            "344 км",
            "355 км",
            "357 км",
            "375 км",
            "398 км",
            "406 км",
            "409 км",
            "414 км",
            "416 км",
            "430 км",
            "439 км",
            "443 км",
            "447 км",
            "450 км",
            "451 км",
            "453 км",
            "454 км",
            "462 км",
            "468 км",
            "471 км",
            "478 км",
            "Авраамовская",
            "Адамово",
            "Адровка",
            "Александрия",
            "Александровка",
            "Алёнушка",
            "Алеща",
            "Андреевичи",
            "Антоновка",
            "Антополь",
            "Дрогичинский  р-н",
            "Антополь",
            "Речицкий р-н",
            "Анусино",
            "Апалоновка",
            "Асановский",
            "Асеевка",
            "Асино",
            "Аульс",
            "Бабино",
            "Бабичи",
            "Баравуха",
            "Барановичи",
            "Барановичи-Полесские",
            "Барановичи-Северные",
            "Барановичи-Центральные",
            "Баркалабово",
            "Барсуки",
            "Минскаяобл.",
            "Барсуки",
            "Могилевский р-н",
            "Бастуны",
            "Батали",
            "Батча",
            "Безверховичи",
            "Беларусь",
            "Белогруда",
            "Белоозерск",
            "Белосельский",
            "Белынковичи",
            "Бель",
            "Бениславского",
            "Беняконе",
            "Бережа",
            "Бережок",
            "Берёза-Картузская",
            "Берёза-Город",
            "Березина",
            "Борисовский р-н",
            "Березина",
            "г.Бобруйск",
            "Березинское",
            "Берёзки",
            "Берёзоваяроща",
            "Берёзовцы",
            "Берестовица",
            "Бибковщина",
            "Бигосово",
            "Бирюличи",
            "Благовичи",
            "Блажевщина",
            "Блужа",
            "Бобр",
            "Бобрич",
            "Бобруйск",
            "Бобры",
            "Жабинковский р-н",
            "Бобры",
            "Мостовский р-н",
            "Богданов",
            "Богутичи",
            "Богушевка",
            "Богушевская",
            "Бокуны",
            "Больница",
            "Бони",
            "Борисов",
            "Борки",
            "Борковичи",
            "Боровая",
            "Боровка",
            "Боровое",
            "Боровцы",
            "Борок",
            "Борречье",
            "Бостынь",
            "Бояры",
            "Брадище",
            "Брест",
            "Брест-Восточный",
            "Брест-Полесский",
            "Брест-Северный",
            "Брест-Центральный",
            "Брест-Южный",
            "Бринево",
            "Брицаловичи",
            "Брожа",
            "Бронислав",
            "БроннаяГора",
            "Брузги",
            "Буда-Кошелевская",
            "Будотень",
            "Будслав",
            "Буды",
            "Буйничи",
            "Бумажково",
            "Бурбин",
            "Буслы",
            "Бухличи",
            "Бушевка",
            "Бытень",
            "Быхов",
            "Бычиха",
            "Ванилевичи",
            "Василевичи",
            "Васьковщизна",
            "Ведрич",
            "Веленский",
            "ВеликиеЛуки",
            "Велино",
            "Вендеж",
            "Вендриж",
            "Верасы",
            "Верба",
            "Верейцы",
            "Веремейки",
            "Вереньки",
            "Веретеи",
            "Веровойша",
            "Верхи",
            "Верхнедвинск",
            "Верхутино",
            "Веселовский",
            "Ветрино",
            "Видибор",
            "Вилейка",
            "Вирский",
            "Витебск",
            "Витебск-Пассажирский",
            "Власье",
            "Влодава",
            "Войгяны",
            "Войтешин",
            "Войтковичи",
            "Волковыск",
            "Волковыск-Город",
            "Волковыск-Центральный",
            "Воловники",
            "Володьки",
            "Воложин",
            "Волоки",
            "Волосковня",
            "Волотова",
            "Волчковичи",
            "Воничи",
            "Вонлярово",
            "Воронино",
            "Вороново",
            "Воропаево",
            "Воротище",
            "Восток",
            "Вулька Антопольская",
            "Вулька Лунинецкая",
            "Выгода",
            "Выдерщина",
            "Выдрея",
            "Вылазы",
            "Высоко-Литовск",
            "Высокое-Город",
            "Вязынка",
            "Вятны",
            "Гавья",
            "Гай",
            "Ганцевичи",
            "Гаути",
            "Геолог",
            "Гибуличи",
            "ГлебоваРудня",
            "Глубокое",
            "Глушанино",
            "Глядки",
            "Гнездово",
            "Годутишки",
            "Голевицы",
            "Голошевка",
            "Голынец",
            "Голынка",
            "Голынки",
            "Гомель",
            "Гомель-Нечётный",
            "Гомель-Пассажирский",
            "Гомель-Северный",
            "Гомель-Чётный",
            "Гомоновка",
            "Гончары",
            "Городец",
            "Городея",
            "Городище",
            "Городнянский",
            "Городок",
            "Городщина",
            "Горожа",
            "Горочичи",
            "Горушки-Невельские",
            "Горынь",
            "Горяны",
            "Грабовка",
            "Гребнево",
            "Грибачи",
            "Гриньки",
            "Грицевец",
            "Гришаны",
            "Гродзянка",
            "Гродно",
            "Грушевка",
            "Гряда",
            "Гудогай",
            "Гуды",
            "Гусино",
            "Гута",
            "Гутно",
            "Далекий",
            "ДаниловМост",
            "Даниловцы",
            "Дараганово",
            "Дарливое",
            "Дары",
            "Дачная",
            "Дачная-1",
            "Дачная-2",
            "Дачное",
            "Дачный",
            "Дашковка",
            "Дворец",
            "Дегтяны",
            "Дедново",
            "Дедовка",
            "Демехи",
            "Депо",
            "Деповской",
            "Деревцы",
            "Детковичи",
            "Дзержинск",
            "Дикаловка",
            "Дитва",
            "Дитрики",
            "Добрик",
            "Добрино",
            "Добруш",
            "Довгердишки",
            "Должа",
            "Доманово",
            "Домановский",
            "Домачево",
            "Домашаны",
            "Домашевичи",
            "Дрануха",
            "Дребск",
            "Дретунь",
            "Дричин",
            "Дрогичин",
            "Дрогичин-Город",
            "Друть",
            "Друя",
            "Дубица",
            "Дубно",
            "Дубновичи",
            "Дубок",
            "Дубравы",
            "Дубровинка",
            "Дубровичи",
            "Дударево",
            "Дудичи",
            "Дятловичи",
            "Дятлово",
            "Езерище",
            "Елизово",
            "Ельск",
            "Еремино",
            "Ермоловка",
            "Ёткишки",
            "Жабинка",
            "Ждановичи",
            "Железница",
            "Железница",
            "Барановичский р-н",
            "Железнодорожный",
            "Желудок",
            "Жемчужина",
            "Жердь",
            "Жеребковичи",
            "Жестянка",
            "Жефарово",
            "Жиличи",
            "Житковичи",
            "Житомля",
            "Жлобин",
            "Жлобин-Западный",
            "Жлобин-Пассажирский",
            "Жлобин-Подольский",
            "Жлобин-Северный",
            "Жлобин-Сортировочный",
            "Жодино",
            "Жодино-Южное",
            "Журавинка",
            "Гомельский р-н",
            "Журавинка",
            "Лунинецкий р-н",
            "Журбин",
            "Журки",
            "Забабье",
            "Забозье",
            "Забойники",
            "Заболотинка",
            "Заболотье",
            "Завадичи",
            "Загатье",
            "Загорбатье",
            "Загорье",
            "Загродье",
            "Задрутье",
            "Зазерка",
            "Заказанка",
            "Закопытье",
            "Закрутин",
            "ЗалесскаяСлобода",
            "Залесье",
            "Заличинка",
            "Залучье",
            "Замосточье",
            "Замошье",
            "Барановичский р-н",
            "Замошье",
            "Глубокский р-н",
            "Замужанье",
            "Заозерщина",
            "Заольша",
            "Западное",
            "Заполье",
            "Заречное",
            "Засковичи",
            "Заслоново",
            "Защобье",
            "Збунин",
            "Звезда",
            "Звёздный",
            "Зелёное",
            "Зелёныйостров",
            "Зельва",
            "Зенит",
            "Злынка",
            "Знаменка",
            "Зори",
            "Зубки",
            "Зубры",
            "Зябки",
            "Зябровка",
            "Зябы",
            "Иваки",
            "Ивацевичи",
            "Ивная",
            "Идолты",
            "Избынь",
            "Изоча",
            "Илово",
            "Ипуть",
            "Ипуть",
            "Гомельский р-н",
            "Исса",
            "Истопки",
            "Кабаки",
            "Казимирово",
            "Каледино",
            "Калий-1",
            "Калий-3",
            "Калининский",
            "Калинковичи",
            "Калинковичи-Восточные",
            "Калинковичи-Западные",
            "Калинковичи-Южные",
            "Калыбовка",
            "Камайка",
            "Каменка",
            "Каменная",
            "Камень",
            "Каплица",
            "Каратай",
            "Кастрычник",
            "Катынь",
            "Кацуры",
            "Качаново",
            "Кветка",
            "Киреево",
            "Кирьяновцы",
            "Киселевичи",
            "Кледневичи",
            "Кленки",
            "Клецк",
            "Климовичи",
            "Клинск",
            "Клинцы",
            "Клишевичи",
            "Клочки",
            "Клыповщина",
            "Клястица",
            "Княгинин",
            "Княжица",
            "Князюковцы",
            "Кобелки",
            "Кобрин",
            "Ковали",
            "Кодень",
            "Козенки",
            "Козловичи",
            "Козлякевичи",
            "Койданово",
            "Колодищи",
            "Колосово",
            "Колядичи",
            "Комаровка",
            "Комарово",
            "Коммунары",
            "КонстантиновДвор",
            "Копань",
            "Копты",
            "Копцевичи",
            "Копысь",
            "Кореневка",
            "Коржевка",
            "Коробчицы",
            "Коровышень",
            "Коссово-Полесское",
            "Костени",
            "Костюковка",
            "Коханово",
            "Кошелево",
            "Кошь",
            "Кравцовка",
            "Краево",
            "КраснаяБуда",
            "КраснаяГорка",
            "Красновка",
            "Красное",
            "КрасноеЗнамя",
            "Красноручье",
            "КрасныйБерег",
            "КрасныйБережок",
            "КрасныйБор",
            "КрасныйБрод",
            "Крестьянка",
            "Кривичи",
            "Криница",
            "Криничино",
            "Кричев-1",
            "Кричев-2",
            "Крошин",
            "Круглица",
            "Круговец",
            "Крулевщизна",
            "Крупки",
            "Крыжовка",
            "Крынки",
            "Ксты",
            "Кулени",
            "Кульгаи",
            "Кульшичи",
            "Куприно",
            "Курасовщина",
            "Курган",
            "Куренец",
            "Куток",
            "Кучкуны",
            "Лазовики",
            "Лазурная",
            "Лапичи",
            "Ларищево",
            "Ластоянцы",
            "Лахва",
            "Лебяды",
            "Лебяжий",
            "Лелеквинская",
            "Лемница",
            "Ленинодар",
            "Лепель",
            "Лесная",
            "Лесники",
            "Летцы",
            "Лида",
            "Лиозно",
            "Липа",
            "Липинки",
            "Липлевка",
            "Липники",
            "Липово",
            "Лисички",
            "Лиски",
            "Листопады",
            "Литва",
            "Лихачи",
            "Лобачи",
            "Лобча",
            "Ловча",
            "Ловша",
            "Логвины",
            "Лозки",
            "Локомотивноедепо",
            "Ломачино",
            "Лосвида",
            "Лосево",
            "Лоси",
            "Лососно",
            "Лотва",
            "Лошица",
            "Лудчицы",
            "Лужесно",
            "Лужки",
            "Лукское",
            "Лукьяновка",
            "Лунинец",
            "Луполово",
            "Лучеса",
            "Лынтупы",
            "Лычковского",
            "Лыщицы",
            "Любань",
            "Любашево",
            "Любожердье",
            "Людиневичи",
            "Люсино",
            "Люта",
            "Люща",
            "Лясы",
            "Ляховичи",
            "Ляховщина",
            "Майский",
            "Макановичи",
            "Малевичи",
            "Малинники",
            "Малорита",
            "Мальковичи",
            "Малятичи",
            "Манюки",
            "Мартюхово",
            "Масюковщина",
            "Матеевичи",
            "Мачулищи",
            "Машково",
            "Машок",
            "Медведка",
            "Мезиновка",
            "Мельковщизна",
            "Мелькомбинат",
            "Мельники",
            "Менютёво",
            "Металлург",
            "Жлобинский р-н",
            "Металлург",
            "Могилевский р-н",
            "Меховое",
            "Микашевичи",
            "Микелевщина",
            "Микуличи",
            "Милое",
            "Мильча",
            "Минойты",
            "Минск",
            "Минск",
            "ИнститутКультуры",
            "Минск-Восточный",
            "Минск-Пассажирский",
            "Минск-Северный",
            "Минск-Южный",
            "МинскоеМоре",
            "Миоры",
            "Мирадино",
            "Митьки",
            "Михалки",
            "Михановичи",
            "Михеевичи",
            "Михново",
            "Мицкевичи",
            "Могилев",
            "Могилёв-1",
            "Могилёв-2",
            "Могилёв-3",
            "Можеевка",
            "Мозырь",
            "Мокрое",
            "Молодёжный",
            "Ганцевичский р-н",
            "Молодёжный",
            "Сморгонский р-н",
            "Молодечно",
            "Молокоедово",
            "Молотковичи",
            "Молчадь",
            "Монтовты",
            "Мордичи",
            "Мормаль",
            "Мороськи",
            "Морочь",
            "Мосты",
            "Мотыкалы",
            "МотыкалыБольшие",
            "Мошны",
            "Мстибово",
            "Муляровка",
            "Мурованка",
            "Мухавец",
            "Мытва",
            "Мышанка",
            "Мясота",
            "Нагораны",
            "Нарцизово",
            "Насилово",
            "Нахов",
            "Национальныйаэропорт«Минск»",
            "Невель2",
            "Невель-1",
            "Невель-2",
            "Негорелое",
            "Некраши",
            "Неман",
            "Неманица",
            "Немейщизна",
            "Несета",
            "Нестеровичи",
            "Нивки",
            "Нивы",
            "Низовцы",
            "Никольск",
            "Никольский",
            "НоваяДуброва",
            "НоваяЖизнь",
            "НоваяНива",
            "НоваяРудня",
            "Новобелицкая",
            "Новогорки",
            "Новодворцы",
            "Новодруцк",
            "НовоеСело",
            "Новоельня",
            "Новозыбков",
            "Новолиповский",
            "Новосады",
            "Новоселки",
            "Новосокольники",
            "Новохованск",
            "НовыеЛуки",
            "Носовичи",
            "Оболь",
            "Огдемер",
            "Оголичи",
            "Оземля",
            "Озерище",
            "Озёрная",
            "Озерница",
            "Октябрьский",
            "Олехновичи",
            "Ольсевичи",
            "Ольховка",
            "Омельня",
            "Опухлики",
            "Оранчицы",
            "Орловичи",
            "Орсичи",
            "Орша",
            "Орша-Восточная",
            "Орша-Западная",
            "Орша-Центральная",
            "Осиновка",
            "Осиповичи-1",
            "Осиповичи-2",
            "Осиповичи-3",
            "Осиповщина",
            "Осмоловичи",
            "Осов",
            "Осовец",
            "Осово",
            "Останковичи",
            "Оточка",
            "Отцеда",
            "Ошмяны",
            "Павловичи",
            "Парафьянов",
            "Парохонск",
            "Паршино",
            "Пасека",
            "Перевалочный",
            "Перевоз",
            "Переможник",
            "Пересельцы",
            "Песчаники",
            "Петуховщина",
            "Петюны",
            "Печинский",
            "Пигановичи",
            "Пинск",
            "Пичевка",
            "Плесы",
            "Плетяничи",
            "Плоская",
            "Плудим",
            "Повстынь",
            "Погодино",
            "Погорелое",
            "Погорельцы",
            "Подлесье",
            "Подозерцы",
            "Подсвилье",
            "Подстанция",
            "Пожежин",
            "Поздняково",
            "Полеваче",
            "Полесскийпарк",
            "Полонка",
            "Полота",
            "Полоцк",
            "Полочаны",
            "Полыковскиехутора",
            "Помыслище",
            "Понизов",
            "Понятичи",
            "Поречаны",
            "Поречье",
            "Порплище",
            "Поселковая",
            "Пост№1",
            "Поставы",
            "Потаповка",
            "Пралески",
            "Прибор",
            "Приборово",
            "Прибужье",
            "Пригородный",
            "Прилуки",
            "Прилутчина",
            "Приозерный",
            "Припять",
            "Приямино",
            "Прокшино",
            "ПролетарскаяПобеда",
            "Прудовка",
            "Прудок",
            "Пруды",
            "Птичь",
            "Калинковичский р-н",
            "Птичь",
            "Минский р-н",
            "Пуховичи",
            "Пуща",
            "Пхов",
            "Пятигорье",
            "Пячковичи",
            "Рабкор",
            "Равнополье",
            "Радеево",
            "Радошковичи",
            "Радуга",
            "Развадово",
            "Разлоги",
            "Ракитная",
            "Ракитно",
            "Раклевцы",
            "Рандовский",
            "Рассвет",
            "Ратмировичи",
            "Ратомка",
            "Ребуса",
            "Рейтанов",
            "Ректа",
            "Ремество",
            "Реста",
            "Речица",
            "Березовский р-н",
            "Речица",
            "Речицкий р-н",
            "Ржачи",
            "Ритм",
            "Робчик",
            "Рогачев",
            "Рожанка",
            "Романовичи",
            "Романовщина",
            "Романы",
            "Роматово",
            "Ропнянская",
            "Росляки",
            "Рось",
            "Роща",
            "Рудавка",
            "Руденск",
            "Рудея",
            "Рудня",
            "Руднянский",
            "Румино",
            "Русино",
            "Рыбница",
            "Рыбцы",
            "Рыжковичи",
            "Савелинки",
            "Савичи",
            "Бобруйский р-н",
            "Савичи",
            "Волковысский р-н",
            "Сад",
            "Садки",
            "Садовый",
            "Сады",
            "Саки",
            "Салатье",
            "Салтановка",
            "Самулки",
            "Сарьянка",
            "Сахарныйзавод",
            "Сверково",
            "Светлогорск-на-Березине",
            "Светоч",
            "Свислочь",
            "Свислочь-посёлок",
            "Свольно",
            "Севрюки",
            "Седча",
            "Секеровщина",
            "Селивоновка",
            "Сельное",
            "Селяхи",
            "Семашки",
            "Семуковичи",
            "Сенкевичи",
            "Сенненский",
            "Сенозавод",
            "Середняки",
            "Сечки",
            "Сидоровка",
            "Синюга",
            "Синяво",
            "Ситница",
            "Ситница-посёлок",
            "Скидель",
            "Скоки",
            "Скрестины",
            "Скрибовцы",
            "Славное",
            "Слобода",
            "Слободка",
            "Слободский",
            "Словечно",
            "Слоним",
            "Слуцк",
            "Случь",
            "Смолевичи",
            "Смоленск",
            "Смольяны",
            "Сморгонь",
            "Снитово",
            "Соболевка",
            "Советский",
            "Сож",
            "Соколка",
            "Солигорск",
            "Солнечный",
            "Солоноя",
            "Солы",
            "Сосница",
            "Сосновка",
            "СосновыйБор",
            "Стайки",
            "Станьково",
            "СтараяВесь",
            "СтараяРудня",
            "Старина",
            "СтароеСело",
            "Старосельский",
            "Старушки",
            "СтарыеДороги",
            "Степянка",
            "Столбцы",
            "Столичный",
            "Столпы",
            "Стоялово",
            "Страдичи",
            "Страж",
            "Строитель",
            "Сураж",
            "Суша",
            "Талька",
            "Татарка",
            "Татарщизна",
            "Тачанка",
            "Тевли",
            "Текстильщик",
            "Телуша",
            "ТемныеЛяды",
            "Темныйлес",
            "Тереховка",
            "Терюха",
            "Техникум",
            "Тимирязево",
            "Тимковичи",
            "Тихиничи",
            "Тишовка",
            "Ткачи",
            "Товарныйдвор",
            "Токари",
            "Толочин",
            "Тощица",
            "Тракторный",
            "Троцилово",
            "Труд",
            "Трудовая",
            "Туголица",
            "Туросна",
            "Турья",
            "Тышкевичи",
            "Уболоть",
            "Уборок",
            "Удрицк",
            "Ужлятино",
            "Уза",
            "Узбережь",
            "Уздорники",
            "Узнаж",
            "Ульяновка",
            "Унеча",
            "Уречье",
            "Уть",
            "Учитель",
            "Уша",
            "Фабричный",
            "Факел",
            "Фаличи",
            "Фаниполь",
            "Фариново",
            "Федюки",
            "Фестивальный",
            "Фомино",
            "Халечино",
            "Хальч",
            "Харитоны",
            "Харсы",
            "Хвоево",
            "Хвоецкое",
            "Хвойняны",
            "Хлусово",
            "Хлюстино",
            "Хмелевка",
            "Ходосы",
            "Хойники",
            "Холодники",
            "Хоново",
            "Хороброво",
            "Хорошки",
            "Хотислав",
            "Цверма",
            "Цель",
            "Центролит",
            "Чаусы",
            "Чашники",
            "Чепино",
            "Червено",
            "Черевачицы",
            "Череток",
            "Черлена",
            "ЧёрнаяНатопа",
            "Черницы",
            "Черное",
            "Чернозём",
            "Черноземовка",
            "Чернуха",
            "ЧерныеБроды",
            "Чижовка",
            "Шаловичи",
            "Шарабаи",
            "Шарковщизна",
            "Шарыбовка",
            "Шахи",
            "Шахтёрский",
            "Шестеровка",
            "Шибеки",
            "Шипуличи",
            "Шипы",
            "Шклов",
            "Шумилино",
            "Шуховцы",
            "Щара",
            "Щежер",
            "Щепичи",
            "Щербовка",
            "Щитники",
            "Энергетик",
            "Юбилейный",
            "Юго-Запад",
            "Юратишки",
            "Юрковка",
            "Юровичи",
            "Юрцево",
            "Юхновичи",
            "Юшкевичи",
            "Юшки",
            "Яжевки",
            "Язвино",
            "Якимовка",
            "Яковчицы",
            "ЯкубаКоласа",
            "Янов-Полесский",
            "Ярево",
            "Ясельда",
            "Ясень",
            "Яхимовщина",
            "Яцуки",
            "Ящицы"
        
            #endregion
        };

        public IEnumerator GetEnumerator()
        {
            return AutoCompletions.GetEnumerator();
        }

        private void suggestions_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason != AutoSuggestionBoxTextChangeReason.UserInput)
                return;
            Suggestions.ItemsSource = null;
            if (sender.Text.Length < 2)
                return;
            Suggestions.ItemsSource = SetSuggestions(sender.Text);
        }

        private void suggestions_TextChanged1(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason != AutoSuggestionBoxTextChangeReason.UserInput)
                return;
            Suggestions1.ItemsSource = null;
            if (sender.Text.Length < 2)
                return;
            Suggestions1.ItemsSource = SetSuggestions(sender.Text);
        }

        private List<string> SetSuggestions(string input)
        {
            return AutoCompletions.Where(city => city.Contains(input)).ToList();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!AutoCompletions.Contains(Suggestions.Text) || !AutoCompletions.Contains(Suggestions1.Text))
            {
                MessageDialog messageDialog = new MessageDialog("Один или оба пункта не существует");
                await messageDialog.ShowAsync();
                return;
            }
            string date = GetDate();
            var schedule = await GetTrainSheldure(Suggestions.Text, Suggestions1.Text, date);
            try
            {
                this.Frame.Navigate(typeof(BlankPage3), schedule);
            }
            catch (Exception)
            {
                //handle the exception here
            }
            finally
            {
                MyIndeterminateProbar.Visibility = Visibility.Collapsed;
            }
        }

        public Task<List<Train>> GetTrainSheldure(string from, string to, string date)
        {
            MyIndeterminateProbar.Visibility = Visibility.Visible;
            return Task.Run(() => TrainGrabber.GetTrainSchedure(from, to, date));
        }
        private string GetDate()
        {
            return DatePicker.Date.Year + "-" + DatePicker.Date.Month + "-" + DatePicker.Date.Day;
        }
    }
}