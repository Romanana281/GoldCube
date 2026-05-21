namespace GoldCube
{
    public class TemplateKeyboards
    {
        public static VkKeyboard Menu()
        {
            return new VkKeyboard
            {
                Buttons =
                    [

                        [

                            new VkButton
                            {
                                Color = "positive",
                                Action = new VkButtonAction {
                                    Label = "Найти беседу",
                                    Payload = "{\"command\": \"FindConv\"}"
                                }
                            }
                        ]
                    ]
            };
        }

        public static VkKeyboard ConvUrl()
        {
            return new VkKeyboard
            {
                Inline = true,
                Buttons =
                    [
                        [
                          new VkButton
                            {
                                Action = new VkButtonAction {
                                    Type = "open_link",
                                    Link = "https://vk.me/join/AuOsv1Z8VLHDr6TeOkEoCUSrj8PtimoxOmw=",
                                    Label = "Wheel"
                                }
                            },
                          new VkButton
                            {
                                Action = new VkButtonAction {
                                    Type = "open_link",
                                    Link = "https://vk.me/join/a_bTaecCu9Ko7avnVaS8owFMzaRVmb4EatE=",
                                    Label = "Dice"
                                }
                            }
                        ]
                    ]
            };
        }

        public static VkKeyboard WheelMenu()
        {
            return new VkKeyboard
            {
                Inline = false,
                Buttons =
                    [

                        [
                            new VkButton
                            {
                                Color = "positive",
                                Action = new VkButtonAction {
                                    Label = "Банк",
                                    Payload = "{\"command\": \"Bank\"}"
                                }
                            },
                            new VkButton
                            {
                                Color = "positive",
                                Action = new VkButtonAction {
                                    Label = "Баланс",
                                    Payload = "{\"command\": \"Balance\"}"
                                }
                            }
                        ],
                        [
                          new VkButton
                            {
                                Color = "primary",
                                Action = new VkButtonAction {
                                    Label = "Красное",
                                    Payload = "{\"command\": \"Bet\", \"type\": \"Red\"}"
                                }
                            },
                          new VkButton
                            {
                                Color = "primary",
                                Action = new VkButtonAction {
                                    Label = "Черное",
                                    Payload = "{\"command\": \"Bet\", \"type\": \"Black\"}"
                                }
                            }
                        ],
                        [
                          new VkButton
                            {
                                Color = "primary",
                                Action = new VkButtonAction {
                                    Label = "Четное",
                                    Payload = "{\"command\": \"Bet\", \"type\": \"Even\"}"
                                }
                            },
                          new VkButton
                            {
                                Color = "secondary",
                                Action = new VkButtonAction {
                                    Label = "На число",
                                    Payload = "{\"command\": \"Bet\", \"type\": \"Number\"}"
                                }
                            },
                          new VkButton
                            {
                                Color = "primary",
                                Action = new VkButtonAction {
                                    Label = "Нечетное",
                                    Payload = "{\"command\": \"Bet\", \"type\": \"Odd\"}"
                                }
                            }
                        ]
                    ]
            };
        }

    }
}