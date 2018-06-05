import React, { Component } from 'react';
import { getFailureDivs } from './HistoryBarProvider'
import './css/testHistory.css';

class TestHistoryBar extends Component {
  constructor() {
    super();
    this.state = {
      failureDivs: []
    };
  }

  componentDidMount() {
    this.setState({
      failureDivs: getFailureDivs(this.props.testFailureDurations, this.historyBarDiv)
    });
  }

  componentWillReceiveProps(props) {
    this.setState({
      failureDivs: getFailureDivs(this.props.testFailureDurations, this.historyBarDiv)
    });
  }

  render() {
    return (
      <div className="testHistoryBar" ref={(div) => this.historyBarDiv = div}>
        {this.state.failureDivs.map((failureDiv) => failureDiv)}      
      </div>
    );
  }
}

export default TestHistoryBar;