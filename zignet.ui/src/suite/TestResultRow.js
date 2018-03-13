import React, { Component } from 'react';
import TestStatusLabel from './TestStatusLabel'

class SuiteResults extends Component {

  render() {
    const testResult = this.props.testResult;
    return (
  	  <tr>
        <td>{testResult.TestName}</td>
        <td><TestStatusLabel failingFromDate={testResult.FailingFromDate} passingFromDate={testResult.PassingFromDate}/></td>
        <td></td>
      </tr>
    );
  }
}

export default SuiteResults;
