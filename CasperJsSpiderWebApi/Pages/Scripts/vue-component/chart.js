Vue.component("chart", {
    data: function () {
        var date = new Date()
        return {
            loading: false,
            chartloading: false,
            hasproduct: false,
            queryForm: {
                Name: '',
                DatePeriod: [new Date(date.setTime(date.getTime() - 3600 * 1000 * 24 * 90)), new Date()]
            },
            queryNameList: [],
            pickerOptions2: {
                shortcuts: [{
                    text: '最近一周',
                    onClick(picker) {
                        const end = new Date();
                        const start = new Date();
                        start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '最近一个月',
                    onClick(picker) {
                        const end = new Date();
                        const start = new Date();
                        start.setTime(start.getTime() - 3600 * 1000 * 24 * 30);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '最近三个月',
                    onClick(picker) {
                        const end = new Date();
                        const start = new Date();
                        start.setTime(start.getTime() - 3600 * 1000 * 24 * 90);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '最近半年',
                    onClick(picker) {
                        const end = new Date();
                        const start = new Date();
                        start.setTime(start.getTime() - 3600 * 1000 * 24 * 180);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '最近一年',
                    onClick(picker) {
                        const end = new Date();
                        const start = new Date();
                        start.setTime(start.getTime() - 3600 * 1000 * 24 * 365);
                        picker.$emit('pick', [start, end]);
                    }
                }]
            }
        }
    },
    template: '<el-container  style="height:100%;width:100%;" v-loading="loading">\
                <el-header>\
                    <el-form :model="queryForm" label-width="80px" @submit.native.prevent>\
                        <el-row>\
                            <el-col :span="5">\
                                <el-form-item label="产品名称">\
                                    <el-input v-model="queryForm.Name" clearable placeholder="产品名称"></el-input>\
                                </el-form-item>\
                            </el-col>\
                            <el-col :span="10">\
                                <el-form-item label="日期段">\
                                     <el-date-picker type="daterange" v-model="queryForm.DatePeriod"  align="right" unlink-panels range-separator="至" placeholder="开始日期" end-placeholder="结束日期" :picker-options="pickerOptions2" style = "width: 100%;" ></el-date-picker>\
                                </el-form-item>\
                            </el-col>\
                            <el-col :span="9">\
                                <el-form-item>\
                                    <el-button type="primary" @click="onSubmit" >查询</el-button>\
                                </el-form-item>\
                            </el-col>\
                        </el-row>\
                    </el-form>\
                </el-header>\
                <el-main style="height:100%;width:100%;padding-top:0;">\
                  <el-container  style="height:100%;width:100%;">\
                    <el-header style="height:85px;">\
                        <el-row id="products">\
                            <template v-for="(itemN,indexN) in queryNameList">\
                                <el-col :span="4" :data-id="itemN.ID" class="pointer blue_action" :title="itemN.Name" @click.native="onLoadChart">\
                                    <div class="product_img"><img :src="itemN.ImgPath" onerror="javascript:this.onerror=null;this.src=\'/Pages/Images/nopic.jpg\'" /></div>\
                                    <div class="pre-line2">{{itemN.Name}}</div>\
                                </el-col>\
                            </template>\
                        </el-row>\
                        <el-row :class="{hide:hasproduct}">\
                            <el-col :span="24" class="err_tip">没有该产品的相关信息！</el-col>\
                        </el-row>\
                    </el-header>\
                    <el-main style="height:100%;width:100%;" id="main" v-loading="chartloading">\
                    </el-main>\
                  </el-container>\
                </el-main>\
               </el-container>'
    ,
    methods: {
        createView: function () {
            this.$root.navselected = this.$route.query.index;
            this.queryForm.Name = '';
            this.queryNameList = [];
            this.echarts = echarts.init(document.getElementById('main'));
            this.hasproduct = true;
            this.echarts.clear();
        },
        loadSearchName: function () {
            this.loading = true
            this.queryNameList = [];
            //$('#products [data-id]').removeClass('blue_action_active');
            this.echarts.clear();
            this.hasproduct = true;
            var params = {
                catalogId: this.$route.query.id,
                name: this.queryForm.Name
            };
            if (this.queryForm.DatePeriod
                && this.queryForm.DatePeriod.length > 1) {
                params.startDate = this.dateFormat(this.queryForm.DatePeriod[0]);
                params.endDate = this.dateFormat(this.queryForm.DatePeriod[1])
            }
            Utils.get('/api/searchproductname', params, function (rs) {
                this.loading = false
                if (rs.Success) {
                    this.queryNameList = rs.Data;
                    if (rs.Data.length > 0) {
                        setTimeout(function () { $('#products [data-id]:first').click(); }, 300);
                    } else {
                        this.hasproduct = false;
                    }
                } else {
                    this.$message.error(rs.Msg);
                }
            }, { context: this });
        },
        onSubmit: function () {
            this.loadSearchName();
        },
        dateFormat: function (date, hours, min, second, ms) {
            return date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate(); //new Date(date.getFullYear(), date.getMonth(), date.getDate(), hours || 0, min || 0, second || 0, ms || 0);
        }
        , onLoadChart: function (e) {
            var $el = $(e.target);
            if (!$el.hasClass('blue_action')) {
                $el = $el.parents('.blue_action:first');
            }
            if ($el.hasClass('blue_action_active')) { return; }
            $el.addClass('blue_action_active').siblings().removeClass('blue_action_active');
            this.chartloading = true;

            var params = {
                catalogId: this.$route.query.id,
                productId: $el.attr('data-id')
            };
            if (this.queryForm.DatePeriod
                && this.queryForm.DatePeriod.length > 1) {
                params.startDate = this.dateFormat(this.queryForm.DatePeriod[0]);
                params.endDate = this.dateFormat(this.queryForm.DatePeriod[1]);
            }
            Utils.get("/api/searchproductweekchart", params, function (rs) {
                this.chartloading = false;
                var data, i, len, xAxis, yAxis, temp;
                if (rs.Success) {
                    //转换数据
                    data = rs.Data;
                    yAxis = [];
                    xAxis = [];
                    for (i = 0, len = data.length; i < len; i++) {
                        temp = data[i];
                        xAxis.push(temp.RankTime);
                        yAxis.push(temp.RankLevel);
                    }
                    this.echarts.setOption({
                        title: {
                            text: '一周排名',
                            subtext: '波动情况'
                        },
                        tooltip: {
                            trigger: 'axis',
                            axisPointer: {
                                type: 'cross',
                                label: {
                                    backgroundColor: '#283b56'
                                }
                            }
                        },
                        xAxis: {
                            type: 'category',
                            axisLine: { // 隐藏X轴
                                show: false
                            },
                            axisTick: { // 隐藏刻度线
                                show: false
                            },
                            boundaryGap: false,
                            data: xAxis
                        },
                        yAxis: {
                            min: 1,
                            max: 100,
                            type: 'value',
                            inverse: true
                        },
                        series: [{
                            data: yAxis,
                            type: 'line'
                        }]
                    });
                } else {
                    this.$message.error(rs.Msg);
                }
            }, { context: this })
        }
    },
    created: function () {
        yepnope.injectCss("/Pages/Css/chart.css");
        yepnope.injectJs("/Pages/Scripts/echarts/4.2.0/echarts-all.js", this.createView.bind(this));
    },
    watch: {
        $route: function () {
            this.createView();
        }
    }
});